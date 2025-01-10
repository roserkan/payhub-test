using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.Constants;
using Payhub.Domain.Entities.AccountManagement;
using Shared.Abstractions.Messaging;
using Shared.CrossCuttingConcerns.Exceptions.Types;

namespace Payhub.Application.Features.Accounts.Commands.Update;

public sealed class UpdateAccountCommandHandler : ICommandHandler<UpdateAccountCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public UpdateAccountCommandHandler(IUnitOfWork unitOfWork) 
        => _unitOfWork = unitOfWork;
    
    public async Task<int> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _unitOfWork.AccountRepository.GetAsync(i => i.Id == request.Id, cancellationToken: cancellationToken);
        if (account == null)
            throw new NotFoundException(ErrorMessages.Account_NotFound);
        
        account.Name = request.Name;
        account.AccountNumber = request.AccountNumber;
        account.IsActive = request.IsActive;
        account.FirstBalance = request.FirstBalance;
        account.Password = request.Password;
        account.PhoneNumber = request.PhoneNumber;
        account.Email = request.Email;
        account.EmailPassword = request.EmailPassword;
        account.EmailImapPassword = request.EmailImapPassword;
        account.MinDepositAmount = request.MinDepositAmount;
        account.MaxDepositAmount = request.MaxDepositAmount;
        account.DailyDepositAmountLimit = request.DailyDepositAmountLimit;
        account.DailyWithdrawAmountLimit = request.DailyWithdrawAmountLimit;
        account.PaymentWayId = request.PaymentWayId;
        account.AffiliateId = request.AffiliateId;
        account.BankId = request.BankId;
        account.AccountType = request.AccountType;
        account.AccountClassification = request.AccountClassification;
        
        await _unitOfWork.AccountRepository.UpdateAsync(account);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return account.Id;
    }
}