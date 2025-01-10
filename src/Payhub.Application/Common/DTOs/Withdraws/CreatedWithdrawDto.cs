namespace Payhub.Application.Common.DTOs.Withdraws;

public sealed record CreatedWithdrawDto
{
    public string Message { get; set; } = null!;
    public string ProcessId { get; set; } = null!;
    public bool Success { get; set; }
}


public sealed record CreatedForAccountWithdrawDto
{
    public string AccountName { get; set; } = null!;
    public string Iban { get; set; } = null!;
}