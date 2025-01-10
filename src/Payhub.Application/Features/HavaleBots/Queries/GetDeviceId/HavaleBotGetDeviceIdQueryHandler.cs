using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.Constants;
using Payhub.Application.Common.DTOs.HavaleBots;
using Payhub.Application.Features.HavaleBots.BotResults;
using Shared.Abstractions.Messaging;
using Shared.CrossCuttingConcerns.Exceptions.Types;

namespace Payhub.Application.Features.HavaleBots.Queries.GetDeviceId;

public sealed class HavaleBotGetDeviceIdQueryHandler : IQueryHandler<HavaleBotGetDeviceIdQuery, BotResult<GetDeviceIdDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public HavaleBotGetDeviceIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<BotResult<GetDeviceIdDto>> Handle(HavaleBotGetDeviceIdQuery query, CancellationToken cancellationToken)
    {
        var device = await _unitOfWork.Devices.GetAsync(i => i.SerialNumber == query.DeviceSerialNumber, cancellationToken: cancellationToken);
        if (device == null) throw new NotFoundException(ErrorMessages.Device_NotFound);
        
        return BotResult<GetDeviceIdDto>.Ok(new GetDeviceIdDto { Id = device.Id });
    }
}