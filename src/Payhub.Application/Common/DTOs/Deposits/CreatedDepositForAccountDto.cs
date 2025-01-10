namespace Payhub.Application.Common.DTOs.Deposits;

public sealed record CreatedDepositForAccountDto
{
    public string AccountName { get; set; } = default!;
    public string Iban { get; set; } = default!;
}