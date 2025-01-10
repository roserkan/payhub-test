using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Features.HavaleBots.BotResults;
using Payhub.Domain.Entities.BotManagement;
using Payhub.Domain.Enums;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.HavaleBotMoves.Commands;

public sealed class CreateHavaleBotMoveQueryHandler : ICommandHandler<CreateHavaleBotMoveCommand, BotResult<int>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateHavaleBotMoveQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<BotResult<int>> Handle(CreateHavaleBotMoveCommand request, CancellationToken cancellationToken)
    {
        var havaleBotMove = new HavaleBotMove
        {
            SenderName = request.SenderName,
            Amount = request.Amount,
            ReceiverAccId = request.ReceiverAccId,
            TransferReceivedDate = request.TransferReceivedDate,
            SecurityKey = request.SecurityKey,
            TryCount = 0,
            Status = HavaleBotMoveStatus.Pending,
        };

        await _unitOfWork.HavaleBotMoves.AddAsync(havaleBotMove);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BotResult<int>.Ok(havaleBotMove.Id);
    }
}