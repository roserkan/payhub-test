using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.Constants;
using Shared.Abstractions.Messaging;
using Shared.CrossCuttingConcerns.Exceptions.Types;

namespace Payhub.Application.Features.Affiliates.Commands.Update;

public sealed class UpdateAffiliateCommandHandler : ICommandHandler<UpdateAffiliateCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public UpdateAffiliateCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<int> Handle(UpdateAffiliateCommand request, CancellationToken cancellationToken)
    {
        var affiliate =
            await _unitOfWork.AffiliateRepository.GetAsync(i => i.Id == request.Id,
                cancellationToken: cancellationToken);
        
        if (affiliate == null)
            throw new NotFoundException(ErrorMessages.Affiliate_NotFound);
        
        affiliate.Name = request.Name;
        affiliate.IsDynamic = request.IsDynamic;
        affiliate.DailyDepositLimit = request.DailyDepositLimit;
        affiliate.DailyWithdrawLimit = request.DailyWithdrawLimit;
        affiliate.MinDepositAmount = request.MinDepositAmount;
        affiliate.MaxDepositAmount = request.MaxDepositAmount;
        affiliate.MinWithdrawAmount = request.MinWithdrawAmount;
        affiliate.MaxWithdrawAmount = request.MaxWithdrawAmount;
        affiliate.IsDepositActive = request.IsDepositActive;
        affiliate.IsWithdrawActive = request.IsWithdrawActive;
        affiliate.CommissionRate = request.CommissionRate;
        
        await _unitOfWork.AffiliateRepository.UpdateAsync(affiliate);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return affiliate.Id;
    }
}