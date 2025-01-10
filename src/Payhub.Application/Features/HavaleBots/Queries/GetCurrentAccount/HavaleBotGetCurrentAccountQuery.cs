using Payhub.Application.Common.DTOs.HavaleBots;
using Payhub.Application.Features.HavaleBots.BotResults;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.HavaleBots.Queries.GetCurrentAccount;

public sealed record HavaleBotGetCurrentAccountQuery(int DeviceId) : IQuery<BotResult<GetActiveAccountDto>>;