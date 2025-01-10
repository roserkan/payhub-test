using Payhub.Application.Common.DTOs.Withdraws;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Withdraws.Queries.GetDetailForAccount;

public sealed record GetWithdrawDetailForAccountQuery : IQuery<WithdrawDetailForAccountDto>
{
    public string ProcessId { get; set; } = null!;
}