using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Devices.Commands.Update;

public sealed record UpdateDeviceCommand(int Id, string Name, string? Description, int? AccountId) : ICommand<int>;