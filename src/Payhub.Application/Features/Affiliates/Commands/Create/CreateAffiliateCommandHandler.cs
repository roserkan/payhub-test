using Payhub.Application.Abstractions.Repositories;
using Payhub.Domain.Entities;
using Payhub.Domain.Entities.AffiliateManagement;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Affiliates.Commands.Create;

public sealed class CreateAffiliateCommandHandler : ICommandHandler<CreateAffiliateCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateAffiliateCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<int> Handle(CreateAffiliateCommand request, CancellationToken cancellationToken)
    {
        var affiliate = new Affiliate
        {
            Name = request.Name,
            IsDynamic = request.IsDynamic,
            DailyDepositLimit = request.DailyDepositLimit,
            DailyWithdrawLimit = request.DailyWithdrawLimit,
            MinDepositAmount = request.MinDepositAmount,
            MaxDepositAmount = request.MaxDepositAmount,
            MinWithdrawAmount = request.MinWithdrawAmount,
            MaxWithdrawAmount = request.MaxWithdrawAmount,
            IsDepositActive = request.IsDepositActive,
            IsWithdrawActive = request.IsWithdrawActive,
            CommissionRate = request.CommissionRate
        };
        
        await _unitOfWork.AffiliateRepository.AddAsync(affiliate);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return affiliate.Id;
    }
}