namespace Payhub.Application.Common.DTOs.Payments;

public sealed record AccountForPaymentDto
{
    public int Id { get; set; }
    public string AccountName { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string BankName { get; set; } = string.Empty;
    public string BankIconUrl { get; set; } = string.Empty;
}