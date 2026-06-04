using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BankingSystemAPI.DTOs;
using BankingSystemAPI.Models;
using BankingSystemAPI.Repositories.Interfaces;

namespace BankingSystemAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AccountController : ControllerBase
{
    private readonly IAccountRepository _accountRepo;

    public AccountController(IAccountRepository accountRepo) => _accountRepo = accountRepo;

    private int GetUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetMyAccounts()
    {
        try
        {
            var accounts = await _accountRepo.GetUserAccountsAsync(GetUserId());
            return Ok(accounts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Failed to retrieve accounts"));
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccount(AccountDto dto)
    {
        try
        {
            var validTypes = new[] { "Savings", "Checking" };
            if (!validTypes.Contains(dto.AccountType))
                return BadRequest(Problem(
                    detail: "Account type must be 'Savings' or 'Checking'.",
                    statusCode: 400,
                    title: "Invalid Account Type"));

            if (dto.InitialDeposit < 0)
                return BadRequest(Problem(
                    detail: "Initial deposit cannot be negative.",
                    statusCode: 400,
                    title: "Invalid Deposit"));

            var account = new Account
            {
                UserId = GetUserId(),
                AccountNumber = GenerateAccountNumber(),
                AccountType = dto.AccountType,
                Balance = dto.InitialDeposit
            };

            var id = await _accountRepo.CreateAccountAsync(account);
            return Ok(new { message = "Account created successfully.", accountId = id, accountNumber = account.AccountNumber });
        }
        catch (Exception ex)
        {
            return StatusCode(500, Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Account Creation Error"));
        }
    }

    private static string GenerateAccountNumber() =>
        "ACC" + DateTime.UtcNow.Ticks.ToString()[^10..];
}