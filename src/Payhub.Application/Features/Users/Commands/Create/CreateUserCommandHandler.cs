using Payhub.Application.Abstractions.Repositories;
using Payhub.Domain.Entities.UserManagement;
using Shared.Abstractions.Messaging;
using Shared.Utils.Helpers;
using Shared.Utils.Security.Hashing;

namespace Payhub.Application.Features.Users.Commands.Create;

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var password = PasswordGenerator.GenerateStrongPassword(12);
        HashingHelper.CreatePasswordHash(password, out var passwordHash, out var passwordSalt);
        var user = new User
        {
            Name = request.Name,
            Username = request.Username,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            IsDeleted = false,
            IsTwoFactorEnabled = false,
            TwoFactorSecret = TwoFactorHelper.GenerateSecretKey(),
            FirstPassword = password,
            UserRoles = new List<UserRole>
            {
                new UserRole
                {
                    RoleId = request.RoleId
                }
            }
        };
        
        await _unitOfWork.UserRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return user.Id;
    }
}