using Payhub.Application.Abstractions.Repositories;
using Payhub.Domain.Entities.SiteManagement;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Infrastructures.Commands.Create;

public sealed class CreateInfrastructureCommandHandler : ICommandHandler<CreateInfrastructureCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateInfrastructureCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
    
    public async Task<int> Handle(CreateInfrastructureCommand request, CancellationToken cancellationToken)
    {
        var infrastructure = new Infrastructure
        {
            Name = request.Name,
            Address = request.Address,
            DepositAddress = request.DepositAddress,
            WithdrawAddress = request.WithdrawAddress
        };
        
        await _unitOfWork.InfrastructureRepository.AddAsync(infrastructure);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return infrastructure.Id;
    }
}