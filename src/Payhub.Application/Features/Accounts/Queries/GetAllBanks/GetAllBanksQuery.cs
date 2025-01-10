using Payhub.Application.Common.DTOs.Accounts;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Accounts.Queries.GetAllBanks;

public sealed record GetAllBanksQuery : IQuery<IEnumerable<BankDto>>
{
}