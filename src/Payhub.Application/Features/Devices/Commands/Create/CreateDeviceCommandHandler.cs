using Payhub.Application.Abstractions.Repositories;
using Payhub.Domain.Entities.BotManagement;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Devices.Commands.Create;

public sealed class CreateDeviceCommandHandler : ICommandHandler<CreateDeviceCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateDeviceCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CreateDeviceCommand request, CancellationToken cancellationToken)
    {
        var device = new Device
        {
            Name = request.Name,
            SerialNumber = request.SerialNumber,
            Description = request.Description,
            AccountId = request.AccountId
        };
        
        await _unitOfWork.Devices.AddAsync(device);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return device.Id;
    }
}