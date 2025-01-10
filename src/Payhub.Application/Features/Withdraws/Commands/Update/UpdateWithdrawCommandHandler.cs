using Microsoft.AspNetCore.Http;
using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.Constants;
using Shared.Abstractions.Messaging;
using Shared.CrossCuttingConcerns.Exceptions.Types;
using Shared.Utils.Security.Extensions;

namespace Payhub.Application.Features.Withdraws.Commands.Update;

public sealed record UpdateWithdrawCommandHandler : ICommandHandler<UpdateWithdrawCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public UpdateWithdrawCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<int> Handle(UpdateWithdrawCommand request, CancellationToken cancellationToken)
    {
        var withdraw = await _unitOfWork.WithdrawRepository.GetAsync(
            predicate: i => i.Id == request.Id,
            enableTracking: true,
            cancellationToken: cancellationToken);
        
        if (withdraw is null)
            throw new NotFoundException(ErrorMessages.Withdraws_NotFound);

        var customer = await _unitOfWork.CustomerRepository.GetAsync(i => i.SiteCustomerId == withdraw.SiteCustomerId, cancellationToken: cancellationToken, enableTracking: true);
        if (customer is null)
            throw new NotFoundException(ErrorMessages.Withdraws_CustomerNotFound);
        
        withdraw.PayedAmount = request.Amount;
        withdraw.AccountId = request.AccountId;
        customer.FullName = request.CustomerFullName;
        withdraw.UpdatedUserId = _httpContextAccessor.HttpContext?.User.GetUserId();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return withdraw.Id;
    }
}