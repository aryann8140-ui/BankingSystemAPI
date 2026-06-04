using Dapper;
using BankingSystemAPI.Data;
using BankingSystemAPI.Models;
using BankingSystemAPI.Repositories.Interfaces;

namespace BankingSystemAPI.Repositories;
public class AccountRepository : IAccountRepository
{
    private readonly DapperContext _context;
    public AccountRepository(DapperContext context) => _context = context;

    public async Task<IEnumerable<Account>> GetUserAccountsAsync(int userId)
    {
        var sql = "SELECT * FROM Accounts WHERE UserId = @UserId AND IsActive = 1";
        using var conn = _context.CreateConnection();
        return await conn.QueryAsync<Account>(sql, new { UserId = userId });
    }
    public async Task<Account?> GetAccountByNumberAsync(string accountNumber)
    {
        var sql = "SELECT * FROM Accounts WHERE AccountNumber = @AccountNumber AND IsActive = 1";
        using var conn = _context.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<Account>(sql, new { AccountNumber = accountNumber });
    }
    public async Task<int> CreateAccountAsync(Account account)
    {
        var sql = @"INSERT INTO Accounts (UserId, AccountNumber, AccountType, Balance)
                    VALUES (@UserId, @AccountNumber, @AccountType, @Balance);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";
        using var conn = _context.CreateConnection();
        return await conn.QuerySingleAsync<int>(sql, account);
    }

    public async Task UpdateBalanceAsync(int accountId, decimal newBalance)
    {
        var sql = "UPDATE Accounts SET Balance = @Balance WHERE Id = @Id";
        using var conn = _context.CreateConnection();
        await conn.ExecuteAsync(sql, new { Balance = newBalance, Id = accountId });
    }
}