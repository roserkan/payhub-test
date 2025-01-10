using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Devices.Commands.Delete;

public sealed record DeleteDeviceCommand(int Id) : ICommand<int>;