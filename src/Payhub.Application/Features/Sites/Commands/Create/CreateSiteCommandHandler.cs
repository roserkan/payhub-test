using Payhub.Application.Abstractions.Repositories;
using Payhub.Domain.Entities.SiteManagement;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Sites.Commands.Create;

public sealed class CreateSiteCommandHandler : ICommandHandler<CreateSiteCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateSiteCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
    
    public async Task<int> Handle(CreateSiteCommand request, CancellationToken cancellationToken)
    {
        var site = new Site
        {
            InfrastructureId = request.InfrastructureId,
            Name = request.Name,
            Address = request.Address,
            SitePaymentWays = request.SitePaymentWays.Select(spw => new SitePaymentWay
            {
                PaymentWayId = spw.PaymentWayId,
                IsActive = spw.IsActive,
                Commission = spw.Commission,
                MinBalanceLimit = spw.MinBalanceLimit,
                MaxBalanceLimit = spw.MaxBalanceLimit,
                ApiKey = Guid.NewGuid().ToString(),
                SecretKey = Guid.NewGuid().ToString()
            }).ToList()
        };
        
        await _unitOfWork.SiteRepository.AddAsync(site);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return site.Id;
    }
}