using Microsoft.AspNetCore.Http;
using Payhub.Application.Abstractions.Repositories;
using Payhub.Domain.Entities.SafeManagement;
using Shared.Abstractions.Messaging;
using Shared.Utils.Security.Extensions;

namespace Payhub.Application.Features.AffiliateSafeMoves.Commands.Create;

public sealed class CreateAffiliateSafeMoveCommandHandler : ICommandHandler<CreateAffiliateSafeMoveCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public CreateAffiliateSafeMoveCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<int> Handle(CreateAffiliateSafeMoveCommand request, CancellationToken cancellationToken)
    {
        var affiliateSafeMove = new AffiliateSafeMove
        {
            AffiliateId = request.AffiliateId,    
            MoveType = request.MoveType,
            TransactionType = request.TransactionType,
            Amount = request.Amount,
            CommissionAmount = request.CommissionAmount,
            CommissionRate = request.CommissionRate,
            Description = request.Description,
            TransactionDate = DateTime.SpecifyKind(request.TransactionDate, DateTimeKind.Unspecified),
            CreatedUserId = _httpContextAccessor.HttpContext!.User.GetUserId(),
        };

        await _unitOfWork.AffiliateSafeMoveRepository.AddAsync(affiliateSafeMove);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return affiliateSafeMove.Id;
    }
}