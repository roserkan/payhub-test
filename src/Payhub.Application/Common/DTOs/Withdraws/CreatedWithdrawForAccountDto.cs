namespace Payhub.Application.Common.DTOs.Withdraws;

public sealed record CreatedWithdrawForAccountDto
{
    public string AccountName { get; set; } = null!;
    public string Iban { get; set; } = null!;
}