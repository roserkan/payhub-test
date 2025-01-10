using Payhub.Application.Common.DTOs.Payments;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Payments.Queries.GetAvailableAccounts;

public sealed record GetAvailableAccountsForPaymentQuery : IQuery<IEnumerable<AccountForPaymentDto>>
{
    public int DepositId { get; set; }
    public decimal Amount { get; set; }
    public int PaymentWayId { get; set; }
    public int SiteId { get; set; }
}