using Payhub.Application.Common.DTOs.Devices;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Devices.Queries.GetAll;

public sealed record GetAllDevicesQuery : IQuery<IEnumerable<DeviceDto>>
{
}