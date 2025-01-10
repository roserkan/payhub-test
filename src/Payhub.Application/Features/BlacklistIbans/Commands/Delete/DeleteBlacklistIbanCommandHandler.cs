using Payhub.Application.Abstractions.Repositories;
using Payhub.Domain.Entities.AccountManagement;
using Shared.Abstractions.Messaging;
using Shared.CrossCuttingConcerns.Exceptions.Types;

namespace Payhub.Application.Features.BlacklistIbans.Commands.Delete;

public sealed class DeleteBlacklistIbanCommandHandler : ICommandHandler<DeleteBlacklistIbanCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public DeleteBlacklistIbanCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(DeleteBlacklistIbanCommand request, CancellationToken cancellationToken)
    {
        var blacklistIban = await _unitOfWork.BlacklistIbanRepository.GetAsync(i => i.Id == request.Id, cancellationToken: cancellationToken);
        if (blacklistIban is null)
            throw new NotFoundException("İban karaliste kaydı bulunamadı.");
        
        await _unitOfWork.BlacklistIbanRepository.DeleteAsync(blacklistIban);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return blacklistIban.Id;
    }
}