using Microsoft.EntityFrameworkCore;
using Payhub.Application.Abstractions.Repositories;
using Payhub.Domain.Entities.AccountManagement;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Sites.Commands.DefineSite;

public sealed class DefineSiteCommandHandler : ICommandHandler<DefineSiteCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public DefineSiteCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    public async Task<int> Handle(DefineSiteCommand request, CancellationToken cancellationToken)
    {
        var account = await _unitOfWork.AccountRepository.GetAsync(i => i.Id == request.AccountId, 
            include: i => i.Include(x => x.AccountSites),
            enableTracking: true, cancellationToken: cancellationToken);
        
        
        account!.AccountSites.Clear();
        
        foreach (var siteId in request.SiteIds)
        {
            account!.AccountSites.Add(new AccountSite
            {
                AccountId = account.Id,
                SiteId = siteId
            });
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return account.Id;
    }
}