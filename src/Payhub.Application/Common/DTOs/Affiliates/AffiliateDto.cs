using Shared.Utils.Responses;

namespace Payhub.Application.Common.DTOs.Affiliates;

public sealed record AffiliateDto
{
    public int Id { get; set; }
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
    public bool DepositLimitExceeded { get; set; }
    public bool WithdrawLimitExceeded { get; set; }
    public decimal CommissionRate { get; set; }
    public DateTime CreatedDate { get; set; }
    public IEnumerable<SelectDto> Sites { get; set; } = new List<SelectDto>();

}