namespace Payhub.Application.Features.Affiliates.DynamicAffiliates.Payox.Models;

public class PayoxCallbackPayload
{
    public string? Hash { get; set; }
    public string? TransactionId { get; set; }
    public string? BankId { get; set; }
    public decimal Amount { get; set; }
    public string? UserId { get; set; }
    public string? Name { get; set; }
    public string? UserName { get; set; }
    public string ProcessId { get; set; } = null!;
    public string? Type { get; set; }   
    public string? ConvertedName { get; set; }
    public string? Bank { get; set; }
    public string? BankAccountName { get; set; }
    public string? BankAccountIban { get; set; }
    public string? AccountName { get; set; }
    public string? Iban { get; set; }
    public string? Status { get; set; }
    public string? StatusReason { get; set; }
}

public class PayoxCallbackReturnDto
{
    public string Message { get; set; } = null!;

    public bool Success { get; set; }
}

