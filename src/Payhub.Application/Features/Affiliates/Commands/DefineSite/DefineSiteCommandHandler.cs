using Microsoft.EntityFrameworkCore;
using Payhub.Application.Abstractions.Repositories;
using Payhub.Domain.Entities.AccountManagement;
using Payhub.Domain.Entities.AffiliateManagement;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Affiliates.Commands.DefineSite;

public sealed class AffiliateDefineSiteCommandHandler : ICommandHandler<AffiliateDefineSiteCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public AffiliateDefineSiteCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    public async Task<int> Handle(AffiliateDefineSiteCommand request, CancellationToken cancellationToken)
    {
        var affiliate = await _unitOfWork.AffiliateRepository.GetAsync(i => i.Id == request.AffiliateId, 
            include: i => i.Include(x => x.AffiliateSites),
            enableTracking: true, cancellationToken: cancellationToken);
        
        
        affiliate!.AffiliateSites.Clear();
        
        foreach (var siteId in request.SiteIds)
        {
            affiliate!.AffiliateSites.Add(new AffiliateSite()
            {
                AffiliateId = affiliate.Id,
                SiteId = siteId
            });
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return affiliate.Id;
    }
}