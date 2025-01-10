using Payhub.Application.Abstractions.Repositories;
using Payhub.Domain.Entities.AccountManagement;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.BlacklistIbans.Commands.Create;

public sealed class CreateBlacklistIbanCommandHandler : ICommandHandler<CreateBlacklistIbanCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateBlacklistIbanCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CreateBlacklistIbanCommand request, CancellationToken cancellationToken)
    {
        var blacklistIban = new BlacklistIban
        {
            Iban = request.Iban.Trim().Replace(" ", "")
        };
        
        await _unitOfWork.BlacklistIbanRepository.AddAsync(blacklistIban);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return blacklistIban.Id;
    }
}