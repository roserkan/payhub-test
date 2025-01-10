using Microsoft.EntityFrameworkCore;
using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.Constants;
using Payhub.Domain.Entities.UserManagement;
using Shared.Abstractions.Messaging;
using Shared.CrossCuttingConcerns.Exceptions.Types;

namespace Payhub.Application.Features.Users.Commands.Update;

public sealed class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public UpdateUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<int> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.GetAsync(i => i.Id == request.Id, 
            include: i => i.Include(x => x.UserRoles),
            enableTracking: true,
            cancellationToken: cancellationToken);
        if (user == null)
            throw new NotFoundException(ErrorMessages.User_NotFound);

        user.Name = request.Name;
        user.Username = request.Username;
        user.IsTwoFactorEnabled = request.IsTwoFactorEnabled;
        user.UserRoles.Clear();
        user.UserRoles.Add(new UserRole
        {
            RoleId = request.RoleId,
            UserId = user.Id
        });
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}