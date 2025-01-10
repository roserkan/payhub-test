using Payhub.Application.Common.DTOs.Accounts;
using Payhub.Domain.Enums;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Accounts.Queries.GetForSelect;

public sealed record GetAccountForSelectQuery : IQuery<IEnumerable<AccountSelectDto>>
{
    public AccountType? AccountType { get; set; }
    public bool? IsActive { get; set; }
    public int? PaymentWayId { get; set; }
}