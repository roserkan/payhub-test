using Microsoft.EntityFrameworkCore;
using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.Constants;
using Payhub.Domain.Entities.SiteManagement;
using Shared.Abstractions.Messaging;
using Shared.CrossCuttingConcerns.Exceptions.Types;

namespace Payhub.Application.Features.Sites.Commands.Update;

public sealed class UpdateSiteCommandHandler : ICommandHandler<UpdateSiteCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public UpdateSiteCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
    
    public async Task<int> Handle(UpdateSiteCommand request, CancellationToken cancellationToken)
    {
        var site = await _unitOfWork.SiteRepository.GetAsync(i => i.Id == request.Id,
            cancellationToken: cancellationToken,
            include: i => i.Include(x => x.SitePaymentWays),
            enableTracking: true);
        
        if (site == null)
            throw new NotFoundException(ErrorMessages.Site_NotFound);
        
        site.Name = request.Name;
        site.Address = request.Address;

        foreach (var spw in site.SitePaymentWays)
        {
            var paymentWay = request.SitePaymentWays.FirstOrDefault(i => i.PaymentWayId == spw.PaymentWayId);
            if (paymentWay == null)
            {
                spw.IsActive = false;
                spw.MinBalanceLimit = 0;
                spw.MaxBalanceLimit = 0;
                spw.Commission = 0;
            }
            else
            {
                spw.IsActive = paymentWay.IsActive;
                spw.MinBalanceLimit = paymentWay.MinBalanceLimit;
                spw.MaxBalanceLimit = paymentWay.MaxBalanceLimit;
                spw.Commission = paymentWay.Commission;
            }
        }
        
        await _unitOfWork.SiteRepository.UpdateAsync(site);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return site.Id;
    }
}