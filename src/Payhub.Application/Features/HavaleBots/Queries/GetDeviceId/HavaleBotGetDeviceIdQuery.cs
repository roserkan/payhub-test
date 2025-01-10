using Payhub.Application.Common.DTOs.HavaleBots;
using Payhub.Application.Features.HavaleBots.BotResults;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.HavaleBots.Queries.GetDeviceId;

public sealed record HavaleBotGetDeviceIdQuery(string DeviceSerialNumber) : IQuery<BotResult<GetDeviceIdDto>>;