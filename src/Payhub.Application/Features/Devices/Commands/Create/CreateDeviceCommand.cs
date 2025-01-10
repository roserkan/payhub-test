using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Devices.Commands.Create;

public sealed record CreateDeviceCommand(string Name, string SerialNumber, string? Description, int? AccountId) : ICommand<int>;