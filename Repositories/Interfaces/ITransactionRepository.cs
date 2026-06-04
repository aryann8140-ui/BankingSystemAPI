using BankingSystemAPI.Models;

namespace BankingSystemAPI.Repositories.Interfaces;
public interface ITransactionRepository
{
    Task<IEnumerable<Transaction>> GetTransactionHistoryAsync(int accountId);
    Task<int> CreateTransactionAsync(Transaction transaction);
}