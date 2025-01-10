using Microsoft.AspNetCore.Http;
using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.Constants;
using Shared.Abstractions.Messaging;
using Shared.CrossCuttingConcerns.Exceptions.Types;
using Shared.Utils.Security.Extensions;

namespace Payhub.Application.Features.Deposits.Commands.Update;

public sealed record UpdateDepositCommandHandler : ICommandHandler<UpdateDepositCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public UpdateDepositCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<int> Handle(UpdateDepositCommand request, CancellationToken cancellationToken)
    {
        var deposit = await _unitOfWork.DepositRepository.GetAsync(
            predicate: i => i.Id == request.Id,
            enableTracking: true,
            cancellationToken: cancellationToken);
        
        if (deposit is null)
            throw new NotFoundException(ErrorMessages.Deposits_NotFound);

        var customer = await _unitOfWork.CustomerRepository.GetAsync(i => i.SiteCustomerId == deposit.SiteCustomerId, cancellationToken: cancellationToken, enableTracking: true);
        if (customer is null)
            throw new NotFoundException(ErrorMessages.Deposits_CustomerNotFound);
        
        deposit.PayedAmount = request.Amount;
        deposit.AccountId = request.AccountId;
        customer.FullName = request.CustomerFullName;
        deposit.UpdatedUserId = _httpContextAccessor.HttpContext?.User.GetUserId();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return deposit.Id;
    }
}