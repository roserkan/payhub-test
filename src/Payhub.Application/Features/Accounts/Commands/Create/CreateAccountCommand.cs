using Payhub.Domain.Enums;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Accounts.Commands.Create;

public sealed record CreateAccountCommand : ICommand<int>
{
    public string Name { get; set; } = null!;
    public string AccountNumber { get; set; } = null!;
    public bool IsActive { get; set; }
    public decimal FirstBalance { get; set; }
    
    public string? Password { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? EmailPassword { get; set; }
    public string? EmailImapPassword { get; set; }
    
    public decimal MinDepositAmount { get; set; }
    public decimal MaxDepositAmount { get; set; }
    public decimal DailyDepositAmountLimit { get; set; }
    public decimal DailyWithdrawAmountLimit { get; set; }
    
    public int PaymentWayId { get; set; }
    public int? AffiliateId { get; set; }
    public int BankId { get; set; }
    public AccountType AccountType { get; set; } 
    public AccountClassification AccountClassification { get; set; } 
}