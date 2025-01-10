using MediatR;
using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.Constants;
using Shared.CrossCuttingConcerns.Exceptions.Types;

namespace Payhub.Application.Features.SiteSafeMoves.Commands.Delete;

public sealed class DeleteSiteSafeMoveCommandHandler : IRequestHandler<DeleteSiteSafeMoveCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public DeleteSiteSafeMoveCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<int> Handle(DeleteSiteSafeMoveCommand request, CancellationToken cancellationToken)
    {
        var siteSafeMove = await _unitOfWork.SiteSafeMoveRepository.GetAsync(i => i.Id == request.Id, cancellationToken: cancellationToken);
        if (siteSafeMove == null)
            throw new NotFoundException(ErrorMessages.SiteSafeMoves_NotFound);

        await _unitOfWork.SiteSafeMoveRepository.DeleteAsync(siteSafeMove);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return siteSafeMove.Id;
    }
}