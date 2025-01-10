using Payhub.Domain.Enums;

namespace Payhub.Application.Common.DTOs.Payments;

public sealed record DepositPaymentDto
{
    public int Id { get; set; }
    public DepositStatus Status { get; set; }
    public string RedirectUrl { get; set; } = string.Empty;
    public AccountForPaymentDto? Account { get; set; }
    public decimal Amount { get; set; }
    public int PaymentWayId { get; set; }
    public int SiteId { get; set; }
    public string? CustomerFullName { get; set; } = string.Empty;
    public string? DynamicAccountName { get; set; }
    public string? DynamicAccountNumber { get; set; }
    public DateTime CreatedDate { get; set; }
}