using Payhub.Application.Abstractions.Repositories;
using Payhub.Domain.Entities.AccountManagement;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Accounts.Commands.Create;

public sealed class CreateAccountCommandHandler : ICommandHandler<CreateAccountCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateAccountCommandHandler(IUnitOfWork unitOfWork) 
        => _unitOfWork = unitOfWork;
    
    public async Task<int> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var account = new Account
        {
            Name = request.Name,
            AccountNumber = request.AccountNumber,
            IsActive = request.IsActive,
            FirstBalance = request.FirstBalance,
            Password = request.Password,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            EmailPassword = request.EmailPassword,
            EmailImapPassword = request.EmailImapPassword,
            MinDepositAmount = request.MinDepositAmount,
            MaxDepositAmount = request.MaxDepositAmount,
            DailyDepositAmountLimit = request.DailyDepositAmountLimit,
            DailyWithdrawAmountLimit = request.DailyWithdrawAmountLimit,
            PaymentWayId = request.PaymentWayId,
            AffiliateId = request.AffiliateId,
            BankId = request.BankId,
            AccountType = request.AccountType,
            AccountClassification = request.AccountClassification
        };
        
        await _unitOfWork.AccountRepository.AddAsync(account);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return account.Id;
    }
}