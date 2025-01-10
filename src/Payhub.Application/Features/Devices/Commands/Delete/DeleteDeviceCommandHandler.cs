using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.Constants;
using Shared.Abstractions.Messaging;
using Shared.CrossCuttingConcerns.Exceptions.Types;

namespace Payhub.Application.Features.Devices.Commands.Delete;

public sealed class DeleteDeviceCommandHandler : ICommandHandler<DeleteDeviceCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public DeleteDeviceCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(DeleteDeviceCommand request, CancellationToken cancellationToken)
    {
        var device = await _unitOfWork.Devices.GetAsync(i => i.Id == request.Id, cancellationToken: cancellationToken);
        if (device == null)
            throw new NotFoundException(ErrorMessages.Device_NotFound);
        
        await _unitOfWork.Devices.DeleteAsync(device);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return device.Id;
    }
}