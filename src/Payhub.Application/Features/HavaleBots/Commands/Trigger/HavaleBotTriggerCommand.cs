using System.Globalization;
using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Abstractions.Services;
using Payhub.Application.Features.HavaleBots.BotResults;
using Payhub.Domain.Enums;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.HavaleBots.Commands.Trigger;

public sealed class HavaleBotTriggerCommand : ICommand<BotResult<int>>;


public sealed class HavaleBotTriggerCommandHandler : ICommandHandler<HavaleBotTriggerCommand, BotResult<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITransactionStatusService _transactionStatusService;

    public HavaleBotTriggerCommandHandler(IUnitOfWork unitOfWork, ITransactionStatusService transactionStatusService)
    {
        _unitOfWork = unitOfWork;
        _transactionStatusService = transactionStatusService;
    }

    public async Task<BotResult<int>> Handle(HavaleBotTriggerCommand request, CancellationToken cancellationToken)
    {
        var botMoves = await _unitOfWork.HavaleBotMoves.GetAllAsync(
            predicate: i => i.Status == HavaleBotMoveStatus.Pending && i.CreatedDate < DateTime.Now.AddMinutes(-30),
            cancellationToken: cancellationToken);

        foreach (var botMove in botMoves)
        {
            botMove.TryCount += 1;

            var name = botMove.SenderName.ToLower(new CultureInfo("tr-TR")); // TODO: türkçe karakterleri düzelt.
            var amount = botMove.Amount;
            var createdDate = botMove.CreatedDate;

            var deposit = await _unitOfWork.DepositRepository
                .GetAsync(i => i.CustomerFullName!.ToLower(new CultureInfo("tr-TR")) == name &&
                               i.Amount == amount
                               && i.CreatedDate < createdDate,
                    cancellationToken: cancellationToken);

            if (deposit != null)
            {
                await _transactionStatusService.UpdateDepositStatusAsync(deposit, DepositStatus.Confirmed, true, null,
                    cancellationToken);
                
                botMove.Status = HavaleBotMoveStatus.Confirmed;
            }
                
        }
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return BotResult<int>.Ok(1);
    }

}