using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Affiliates.Commands.Create;

public sealed record CreateAffiliateCommand : ICommand<int>
{
    public string Name { get; set; } = string.Empty;
    public bool IsDynamic { get; set; }
    public decimal DailyDepositLimit { get; set; }
    public decimal DailyWithdrawLimit { get; set; }
    public decimal MinDepositAmount { get; set; }
    public decimal MaxDepositAmount { get; set; }
    public decimal MinWithdrawAmount { get; set; }
    public decimal MaxWithdrawAmount { get; set; }
    public bool IsDepositActive { get; set; }
    public bool IsWithdrawActive { get; set; }
    public decimal CommissionRate { get; set; }
}
