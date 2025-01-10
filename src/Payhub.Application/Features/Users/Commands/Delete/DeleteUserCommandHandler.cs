using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.Constants;
using Shared.Abstractions.Messaging;
using Shared.CrossCuttingConcerns.Exceptions.Types;

namespace Payhub.Application.Features.Users.Commands.Delete;

public sealed class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public DeleteUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    public async Task<int> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.GetAsync(i => i.Id == request.Id, cancellationToken: cancellationToken);
        if (user == null)
            throw new NotFoundException(ErrorMessages.User_NotFound);

        user.IsDeleted = true;
        await _unitOfWork.UserRepository.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return user.Id;
    }
}