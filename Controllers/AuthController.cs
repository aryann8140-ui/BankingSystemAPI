using Microsoft.AspNetCore.Mvc;
using BankingSystemAPI.DTOs;
using BankingSystemAPI.Models;
using BankingSystemAPI.Repositories.Interfaces;
using BankingSystemAPI.Services;

namespace BankingSystemAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthRepository _authRepo;
    private readonly TokenService _tokenService;

    public AuthController(IAuthRepository authRepo, TokenService tokenService)
    {
        _authRepo = authRepo;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        try
        {
            var existing = await _authRepo.GetUserByEmailAsync(dto.Email);
            if (existing != null)
                return BadRequest(Problem(
                    detail: "This email is already registered.",
                    statusCode: 400,
                    title: "Registration Failed"));

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            var id = await _authRepo.RegisterUserAsync(user);
            return Ok(new { message = "User registered successfully.", userId = id });
        }
        catch (Exception ex)
        {
            return StatusCode(500, Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Registration Error"));
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        try
        {
            var user = await _authRepo.GetUserByEmailAsync(dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Unauthorized(Problem(
                    detail: "Invalid email or password.",
                    statusCode: 401,
                    title: "Authentication Failed"));

            var token = _tokenService.GenerateToken(user);
            return Ok(new { token, user = new { user.Id, user.FullName, user.Email, user.Role } });
        }
        catch (Exception ex)
        {
            return StatusCode(500, Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Login Error"));
        }
    }
}