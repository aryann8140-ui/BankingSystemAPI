namespace BankingSystemAPI.DTOs;
public class AccountDto
{
    public string AccountType { get; set; } = string.Empty; 
    public decimal InitialDeposit { get; set; }
}