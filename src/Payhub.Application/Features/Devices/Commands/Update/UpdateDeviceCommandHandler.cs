using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.Constants;
using Shared.Abstractions.Messaging;
using Shared.CrossCuttingConcerns.Exceptions.Types;

namespace Payhub.Application.Features.Devices.Commands.Update;

public sealed class UpdateDeviceCommandHandler : ICommandHandler<UpdateDeviceCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public UpdateDeviceCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(UpdateDeviceCommand request, CancellationToken cancellationToken)
    {
        var device = await _unitOfWork.Devices.GetAsync(i => i.Id == request.Id, cancellationToken: cancellationToken);
        if (device == null)
            throw new NotFoundException(ErrorMessages.Device_NotFound);
        
        device.Name = request.Name;
        device.Description = request.Description;
        device.AccountId = request.AccountId;
        
        await _unitOfWork.Devices.UpdateAsync(device);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return device.Id;
    }
}