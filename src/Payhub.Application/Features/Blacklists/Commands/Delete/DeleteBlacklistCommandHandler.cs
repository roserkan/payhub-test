using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.Constants;
using Shared.Abstractions.Messaging;
using Shared.CrossCuttingConcerns.Exceptions.Types;

namespace Payhub.Application.Features.Blacklists.Commands.Delete;

public sealed class DeleteBlacklistCommandHandler : ICommandHandler<DeleteBlacklistCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public DeleteBlacklistCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
    
    public async Task<int> Handle(DeleteBlacklistCommand request, CancellationToken cancellationToken)
    {
        var blacklist = await _unitOfWork.BlacklistRepository.GetAsync(i => i.Value == request.PanelCustomerId, cancellationToken: cancellationToken);
        
        if (blacklist == null)
            throw new NotFoundException(ErrorMessages.Blacklist_NotFound);
        
        await _unitOfWork.BlacklistRepository.DeleteAsync(blacklist);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return blacklist.Id;
    }
}