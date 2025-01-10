namespace Payhub.Application.Common.DTOs.HavaleBots;

public class GetActiveAccountDto
{
    public int? AccountId { get; set; }
    public int AccountDetailId { get; set; }
    public string? AccountName { get; set; }
    public string? AccountNumber { get; set; }
    public string? AccountPassword { get; set; }
    public string? AccountPhoneNumber { get; set; }
    public string BankName { get; set; }
    public int? AccountClassificationId { get; set; }
}