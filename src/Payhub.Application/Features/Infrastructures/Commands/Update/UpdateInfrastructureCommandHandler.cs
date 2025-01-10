using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.Constants;
using Shared.Abstractions.Messaging;
using Shared.CrossCuttingConcerns.Exceptions.Types;

namespace Payhub.Application.Features.Infrastructures.Commands.Update;

public sealed class UpdateInfrastructureCommandHandler : ICommandHandler<UpdateInfrastructureCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public UpdateInfrastructureCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
    
    public async Task<int> Handle(UpdateInfrastructureCommand request, CancellationToken cancellationToken)
    {
        var infrastructure =
            await _unitOfWork.InfrastructureRepository.GetAsync(i => i.Id == request.Id,
                cancellationToken: cancellationToken);
        
        if (infrastructure == null)
            throw new NotFoundException(ErrorMessages.Infrastructure_NotFound);
        
        infrastructure.Name = request.Name;
        infrastructure.Address = request.Address;
        infrastructure.DepositAddress = request.DepositAddress;
        infrastructure.WithdrawAddress = request.WithdrawAddress;
        
        await _unitOfWork.InfrastructureRepository.UpdateAsync(infrastructure);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return infrastructure.Id;
    }
}