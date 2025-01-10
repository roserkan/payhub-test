using Payhub.Application.Common.DTOs.Accounts;

using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Accounts.Queries.GetAll;

public sealed record GetAllAccountsQuery : IQuery<IEnumerable<AccountDto>>
{
    public int? PaymentWayId { get; set; }
}