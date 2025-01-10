using MediatR;
using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.Constants;
using Shared.CrossCuttingConcerns.Exceptions.Types;

namespace Payhub.Application.Features.AffiliateSafeMoves.Commands.Delete;

public sealed class DeleteAffiliateSafeMoveCommandHandler : IRequestHandler<DeleteAffiliateSafeMoveCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public DeleteAffiliateSafeMoveCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<int> Handle(DeleteAffiliateSafeMoveCommand request, CancellationToken cancellationToken)
    {
        var affiliateSafeMove = await _unitOfWork.AffiliateSafeMoveRepository.GetAsync(i => i.Id == request.Id, cancellationToken: cancellationToken);
        if (affiliateSafeMove == null)
            throw new NotFoundException(ErrorMessages.AffiliateSafeMoves_NotFound);

        await _unitOfWork.AffiliateSafeMoveRepository.DeleteAsync(affiliateSafeMove);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return affiliateSafeMove.Id;
    }
}