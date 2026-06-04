using BankingSystemAPI.Models;

namespace BankingSystemAPI.Repositories.Interfaces;
public interface IAuthRepository
{
    Task<User?> GetUserByEmailAsync(string email);
    Task<int> RegisterUserAsync(User user);
}