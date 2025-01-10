using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.Constants;
using Shared.Abstractions.Messaging;
using Shared.CrossCuttingConcerns.Exceptions.Types;

namespace Payhub.Application.Features.Accounts.Commands.UnDelete;

public sealed class UnDeleteAccountCommandHandler : ICommandHandler<UnDeleteAccountCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public UnDeleteAccountCommandHandler(IUnitOfWork unitOfWork) 
        => _unitOfWork = unitOfWork;
    
    public async Task<int> Handle(UnDeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _unitOfWork.AccountRepository.GetAsync(i => i.Id == request.Id, cancellationToken: cancellationToken);
        if (account == null)
            throw new NotFoundException(ErrorMessages.Account_NotFound);
        
        account.IsDeleted = false;
        
        await _unitOfWork.AccountRepository.UpdateAsync(account);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return account.Id;
    }
}