using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.Constants;
using Payhub.Application.Common.DTOs.Deposits;
using Payhub.Application.Common.DTOs.Payments;
using Shared.Abstractions.Messaging;
using Shared.CrossCuttingConcerns.Exceptions.Types;

namespace Payhub.Application.Features.Payments.Queries.GetDepositForPayment;

public sealed class GetDepositForPaymentQueryHandler : IQueryHandler<GetDepositForPaymentQuery, DepositPaymentDto>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public GetDepositForPaymentQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<DepositPaymentDto> Handle(GetDepositForPaymentQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.DepositRepository.GetWithSelectorAsync(
            d => d.PaymentId == request.PaymentId,
            selector: d => new DepositPaymentDto
            {
                Id = d.Id,
                Status = d.Status,
                RedirectUrl = d.RedirectUrl ?? string.Empty,
                Account = d.Account != null ? new AccountForPaymentDto
                {
                    Id = d.Account!.Id,
                    AccountName = d.Account.Name,
                    AccountNumber = d.Account.AccountNumber,
                    BankName = d.Account.Bank.Name,
                    BankIconUrl = d.Account.Bank.IconUrl
                } : null,
                Amount = d.Amount,
                PaymentWayId = d.PaymentWayId,
                SiteId = d.SiteId,
                CustomerFullName = d.CustomerFullName,
                DynamicAccountName = d.DynamicAccountName,
                DynamicAccountNumber = d.DynamicAccountNumber,
                CreatedDate = d.CreatedDate
                
            }, cancellationToken: cancellationToken);

        if (result == null)
            throw new NotFoundException(ErrorMessages.Deposits_NotFound);
        
        return result;
    }
}