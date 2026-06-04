namespace BankingSystemAPI.Models;
public class Transaction
{
    public int Id { get; set; }
    public int FromAccountId { get; set; }
    public int ToAccountId { get; set; }
    public decimal Amount { get; set; }
    public string TransactionType { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime TransactionDate { get; set; }
}