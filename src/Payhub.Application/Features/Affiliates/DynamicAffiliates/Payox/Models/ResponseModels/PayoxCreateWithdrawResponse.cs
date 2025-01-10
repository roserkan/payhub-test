namespace Payhub.Application.Features.Affiliates.DynamicAffiliates.Payox.Models.ResponseModels;

public class PayoxCreateWithdrawResponse
{
    public string TransactionId { get; set; } = null!;
    public string ProcessId { get; set; } = null!;
    public string BankId { get; set; } = null!;
    public decimal Amount { get; set; }
    public string UserId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string ConvertedName { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string Bank { get; set; } = null!;
    public string AccountName { get; set; } = null!;
    public string Iban { get; set; } = null!;
    public string Hash { get; set; } = null!;
}