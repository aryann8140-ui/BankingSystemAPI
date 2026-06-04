using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BankingSystemAPI.DTOs;
using BankingSystemAPI.Models;
using BankingSystemAPI.Repositories.Interfaces;

namespace BankingSystemAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TransactionController : ControllerBase
{
    private readonly IAccountRepository _accountRepo;
    private readonly ITransactionRepository _transactionRepo;

    public TransactionController(IAccountRepository accountRepo, ITransactionRepository transactionRepo)
    {
        _accountRepo = accountRepo;
        _transactionRepo = transactionRepo;
    }

    [HttpPost]
    public async Task<IActionResult> DoTransaction(TransactionDto dto)
    {
        try
        {
            if (dto.Amount <= 0)
                return BadRequest(Problem(
                    detail: "Transaction amount must be greater than zero.",
                    statusCode: 400,
                    title: "Invalid Amount"));

            var fromAccount = await _accountRepo.GetAccountByNumberAsync(dto.FromAccountNumber);
            if (fromAccount == null)
                return NotFound(Problem(
                    detail: $"Account '{dto.FromAccountNumber}' not found.",
                    statusCode: 404,
                    title: "Account Not Found"));

            var transaction = new Transaction
            {
                FromAccountId = fromAccount.Id,
                Amount = dto.Amount,
                TransactionType = dto.TransactionType,
                Description = dto.Description
            };

            switch (dto.TransactionType.ToLower())
            {
                case "deposit":
                    await _accountRepo.UpdateBalanceAsync(fromAccount.Id, fromAccount.Balance + dto.Amount);
                    transaction.ToAccountId = fromAccount.Id;
                    break;

                case "withdrawal":
                    if (fromAccount.Balance < dto.Amount)
                        return BadRequest(Problem(
                            detail: "Insufficient funds for this withdrawal.",
                            statusCode: 400,
                            title: "Insufficient Funds"));

                    await _accountRepo.UpdateBalanceAsync(fromAccount.Id, fromAccount.Balance - dto.Amount);
                    transaction.ToAccountId = fromAccount.Id;
                    break;

                case "transfer":
                    if (string.IsNullOrEmpty(dto.ToAccountNumber))
                        return BadRequest(Problem(
                            detail: "A target account number is required for transfers.",
                            statusCode: 400,
                            title: "Missing Target Account"));

                    var toAccount = await _accountRepo.GetAccountByNumberAsync(dto.ToAccountNumber);
                    if (toAccount == null)
                        return NotFound(Problem(
                            detail: $"Target account '{dto.ToAccountNumber}' not found.",
                            statusCode: 404,
                            title: "Target Account Not Found"));

                    if (fromAccount.Balance < dto.Amount)
                        return BadRequest(Problem(
                            detail: "Insufficient funds for this transfer.",
                            statusCode: 400,
                            title: "Insufficient Funds"));

                    await _accountRepo.UpdateBalanceAsync(fromAccount.Id, fromAccount.Balance - dto.Amount);
                    await _accountRepo.UpdateBalanceAsync(toAccount.Id, toAccount.Balance + dto.Amount);
                    transaction.ToAccountId = toAccount.Id;
                    break;

                default:
                    return BadRequest(Problem(
                        detail: "Transaction type must be 'Deposit', 'Withdrawal', or 'Transfer'.",
                        statusCode: 400,
                        title: "Invalid Transaction Type"));
            }

            var txId = await _transactionRepo.CreateTransactionAsync(transaction);
            return Ok(new { message = "Transaction completed successfully.", transactionId = txId });
        }
        catch (Exception ex)
        {
            return StatusCode(500, Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Transaction Error"));
        }
    }

    [HttpGet("history/{accountNumber}")]
    public async Task<IActionResult> GetHistory(string accountNumber)
    {
        try
        {
            var account = await _accountRepo.GetAccountByNumberAsync(accountNumber);
            if (account == null)
                return NotFound(Problem(
                    detail: $"Account '{accountNumber}' not found.",
                    statusCode: 404,
                    title: "Account Not Found"));

            var history = await _transactionRepo.GetTransactionHistoryAsync(account.Id);
            return Ok(history);
        }
        catch (Exception ex)
        {
            return StatusCode(500, Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Failed to Retrieve History"));
        }
    }
}