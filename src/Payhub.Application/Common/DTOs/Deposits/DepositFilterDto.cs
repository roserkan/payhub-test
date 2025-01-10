using Payhub.Domain.Enums;
using Shared.Utils.Requests;

namespace Payhub.Application.Common.DTOs.Deposits;

public sealed class DepositFilterDto : DateFilterDto
{
    public int? SiteId { get; set; }
    public int? AccountId { get; set; }
    public int PaymentWayId { get; set; }
    public DepositStatus? Status { get; set; }
    public string? SearchValue { get; set; }
}