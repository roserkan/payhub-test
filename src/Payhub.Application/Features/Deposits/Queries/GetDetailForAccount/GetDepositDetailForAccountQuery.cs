using Payhub.Application.Common.DTOs.Deposits;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Deposits.Queries.GetDetailForAccount;

public sealed record GetDepositDetailForAccountQuery : IQuery<DepositDetailForAccountDto>
{
    public string ProcessId { get; set; } = null!;
}