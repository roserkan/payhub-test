using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.Constants;
using Payhub.Application.Common.DTOs.HavaleBots;
using Payhub.Application.Features.HavaleBots.BotResults;
using Shared.Abstractions.Messaging;
using Shared.CrossCuttingConcerns.Exceptions.Types;

namespace Payhub.Application.Features.HavaleBots.Queries.GetCurrentAccount;

public sealed class HavaleBotGetCurrentAccountQueryHandler : IQueryHandler<HavaleBotGetCurrentAccountQuery, BotResult<GetActiveAccountDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public HavaleBotGetCurrentAccountQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<BotResult<GetActiveAccountDto>> Handle(HavaleBotGetCurrentAccountQuery query, CancellationToken cancellationToken)
    {
        var device = await _unitOfWork.Devices.GetWithSelectorAsync(i => i.Id == query.DeviceId, 
            selector: device => new GetActiveAccountDto()
            {
                AccountId = device.AccountId,
                AccountDetailId = 0,
                AccountName = device.Account.Name,
                AccountNumber = device.Account.AccountNumber,
                AccountPassword = device.Account.Password, // banka giriş şifresi
                AccountPhoneNumber = device.Account.PhoneNumber,
                BankName = device.Account.Bank.Name,
                AccountClassificationId = (int)device.Account.AccountClassification
            },
            cancellationToken: cancellationToken);
        if (device == null) throw new NotFoundException(ErrorMessages.Device_NotFound);
        if (device.AccountId == null) throw new NotFoundException(ErrorMessages.Device_NotFound);
        
        return BotResult<GetActiveAccountDto>.Ok(device);
    }
}