using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.Constants;
using Payhub.Domain.Entities.AccountManagement;
using Shared.Abstractions.Messaging;
using Shared.CrossCuttingConcerns.Exceptions.Types;

namespace Payhub.Application.Features.Accounts.Commands.Delete;

public sealed class DeleteAccountCommandHandler : ICommandHandler<DeleteAccountCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public DeleteAccountCommandHandler(IUnitOfWork unitOfWork) 
        => _unitOfWork = unitOfWork;
    
    public async Task<int> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _unitOfWork.AccountRepository.GetAsync(i => i.Id == request.Id, cancellationToken: cancellationToken);
        if (account == null)
            throw new NotFoundException(ErrorMessages.Account_NotFound);
        
        account.IsDeleted = true;
        
        await _unitOfWork.AccountRepository.UpdateAsync(account);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return account.Id;
    }
}