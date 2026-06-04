using BankingSystemAPI.Models;

namespace BankingSystemAPI.Repositories.Interfaces;
public interface IAccountRepository
{
    Task<IEnumerable<Account>> GetUserAccountsAsync(int userId);
    Task<Account?> GetAccountByNumberAsync(string accountNumber);
    Task<int> CreateAccountAsync(Account account);
    Task UpdateBalanceAsync(int accountId, decimal newBalance);
}