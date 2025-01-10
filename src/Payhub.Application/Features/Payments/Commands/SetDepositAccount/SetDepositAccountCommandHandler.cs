using MediatR;
using Microsoft.EntityFrameworkCore;
using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.Constants;
using Payhub.Domain.Enums;
using Shared.CrossCuttingConcerns.Exceptions.Types;

namespace Payhub.Application.Features.Payments.Commands.SetDepositAccount;

public sealed class SetDepositAccountCommandHandler : IRequestHandler<SetDepositAccountCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public SetDepositAccountCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(SetDepositAccountCommand request, CancellationToken cancellationToken)
    {
        var deposit = await _unitOfWork.DepositRepository.GetAsync(i => i.Id == request.DepositId, cancellationToken: cancellationToken);
        var account = await _unitOfWork.AccountRepository.GetAsync(i => i.Id == request.AccountId, cancellationToken: cancellationToken);

        if (deposit == null)
            throw new NotFoundException(ErrorMessages.Deposits_NotFound);
        
        if (account == null)
            throw new NotFoundException(ErrorMessages.Account_NotFound);
        
        if (deposit.PaymentWayId == (int)PaymentWayEnum.Papara) // Papara ise
            deposit.AccountId = request.AccountId;

        if (deposit.PaymentWayId == (int)PaymentWayEnum.Havale) // Havale ise
        {
            var now = DateTime.Now;
            var eftStartTime = new TimeSpan(9, 30, 0); // 09:30:00 => EFT BASLANGIC
            var eftEndTime = new TimeSpan(16, 50, 0); // 16:50:00 => EFT BİTİŞ
        
            if (now.TimeOfDay < eftStartTime || now.TimeOfDay > eftEndTime) // EFT saatleri dışında ise
            {
                deposit.AccountId = request.AccountId;
            }
            else // // EFT saatlerinde ise farklı banka
            {
                var accounts = await _unitOfWork.AccountRepository.GetAllAsync(predicate:
                    i => i.MinDepositAmount <= deposit!.Amount &&
                         i.MaxDepositAmount >= deposit.Amount &&
                         i.IsActive &&
                         i.AccountType == AccountType.Yatirim &&
                         i.PaymentWayId == deposit.PaymentWayId && 
                         i.AccountSites.Any(x => x.SiteId == deposit.SiteId),
                    include: i => i.Include(x => x.Bank),
                    cancellationToken: cancellationToken
                );
                var acc = accounts.FirstOrDefault(i => i.Id != request.AccountId);
                if (acc == null)
                    deposit.AccountId = request.AccountId;
                else
                    deposit.AccountId = acc.Id;
            }
        }
        
        await _unitOfWork.DepositRepository.UpdateAsync(deposit);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (deposit.AccountId == 0 || deposit.AccountId == null)
            throw new BusinessException("Payment error");
        
        return deposit.AccountId.Value;
    }
}