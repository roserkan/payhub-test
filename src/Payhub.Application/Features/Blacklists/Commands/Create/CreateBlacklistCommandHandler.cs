using Payhub.Application.Abstractions.Repositories;
using Payhub.Domain.Entities.CustomerManagement;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Blacklists.Commands.Create;

public sealed class CreateBlacklistCommandHandler : ICommandHandler<CreateBlacklistCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateBlacklistCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
    
    public async Task<int> Handle(CreateBlacklistCommand request, CancellationToken cancellationToken)
    {
        var blacklist = new Blacklist
        {
            BlacklistType = request.BlacklistType,
            Value = request.Value
        };
        
        await _unitOfWork.BlacklistRepository.AddAsync(blacklist);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return blacklist.Id;
    }
}