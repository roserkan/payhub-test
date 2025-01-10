using Microsoft.AspNetCore.Http;
using Payhub.Application.Abstractions.Repositories;
using Payhub.Domain.Entities.SafeManagement;
using Shared.Abstractions.Messaging;
using Shared.Utils.Security.Extensions;

namespace Payhub.Application.Features.SiteSafeMoves.Commands.Create;

public sealed class CreateSiteSafeMoveCommandHandler : ICommandHandler<CreateSiteSafeMoveCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public CreateSiteSafeMoveCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<int> Handle(CreateSiteSafeMoveCommand request, CancellationToken cancellationToken)
    {
        var siteSafeMove = new SiteSafeMove
        {
            SiteId = request.SiteId,    
            MoveType = request.MoveType,
            TransactionType = request.TransactionType,
            Amount = request.Amount,
            CommissionAmount = request.CommissionAmount,
            CommissionRate = request.CommissionRate,
            Description = request.Description,
            TransactionDate = DateTime.SpecifyKind(request.TransactionDate, DateTimeKind.Unspecified),
            CreatedUserId = _httpContextAccessor.HttpContext!.User.GetUserId(),
        };

        await _unitOfWork.SiteSafeMoveRepository.AddAsync(siteSafeMove);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return siteSafeMove.Id;
    }
}