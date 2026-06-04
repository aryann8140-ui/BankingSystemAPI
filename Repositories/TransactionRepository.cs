using Dapper;
using BankingSystemAPI.Data;
using BankingSystemAPI.Models;
using BankingSystemAPI.Repositories.Interfaces;

namespace BankingSystemAPI.Repositories;
public class TransactionRepository : ITransactionRepository
{
    private readonly DapperContext _context;

    public TransactionRepository(DapperContext context) => _context = context;

    public async Task<IEnumerable<Transaction>> GetTransactionHistoryAsync(int accountId)
    {
        var sql = @"SELECT * FROM Transactions
                    WHERE FromAccountId = @AccountId OR ToAccountId = @AccountId
                    ORDER BY TransactionDate DESC";
        using var conn = _context.CreateConnection();
        return await conn.QueryAsync<Transaction>(sql, new { AccountId = accountId });
    }

    public async Task<int> CreateTransactionAsync(Transaction transaction)
    {
        var sql = @"INSERT INTO Transactions (FromAccountId, ToAccountId, Amount, TransactionType, Description)
                    VALUES (@FromAccountId, @ToAccountId, @Amount, @TransactionType, @Description);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";
        using var conn = _context.CreateConnection();
        return await conn.QuerySingleAsync<int>(sql, transaction);
    }
}