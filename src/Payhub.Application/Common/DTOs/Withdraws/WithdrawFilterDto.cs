using Payhub.Domain.Enums;
using Shared.Utils.Requests;

namespace Payhub.Application.Common.DTOs.Withdraws;

public sealed class WithdrawFilterDto : DateFilterDto
{
    public int? SiteId { get; set; }
    public int? AccountId { get; set; }
    public int PaymentWayId { get; set; }
    public WithdrawStatus? Status { get; set; }
    public string? SearchValue { get; set; }
}