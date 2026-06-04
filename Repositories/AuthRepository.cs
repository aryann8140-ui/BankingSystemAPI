using Dapper;
using BankingSystemAPI.Data;
using BankingSystemAPI.Models;
using BankingSystemAPI.Repositories.Interfaces;

namespace BankingSystemAPI.Repositories;
public class AuthRepository : IAuthRepository
{
    private readonly DapperContext _context;
    public AuthRepository(DapperContext context)
    {
        _context = context;
    }
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        var sql = "SELECT * FROM Users WHERE Email = @Email";
        using var conn = _context.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
    }
    public async Task<int> RegisterUserAsync(User user)
    {
        var sql = @"INSERT INTO Users (FullName, Email, PasswordHash, Role)
                    VALUES (@FullName, @Email, @PasswordHash, @Role);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";
        using var conn = _context.CreateConnection();
        return await conn.QuerySingleAsync<int>(sql, user);
    }
}