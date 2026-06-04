namespace BankingSystemAPI.DTOs;
public class TransactionDto
{
    public string FromAccountNumber { get; set; } = string.Empty;
    public string? ToAccountNumber { get; set; }
    public decimal Amount { get; set; }
    public string TransactionType { get; set; } = string.Empty; 
    public string? Description { get; set; }
}