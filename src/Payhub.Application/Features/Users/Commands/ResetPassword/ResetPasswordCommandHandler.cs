using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.Constants;
using Shared.Abstractions.Messaging;
using Shared.CrossCuttingConcerns.Exceptions.Types;
using Shared.Utils.Security.Hashing;

namespace Payhub.Application.Features.Users.Commands.ResetPassword;

public sealed class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public ResetPasswordCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<int> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.GetAsync(i => i.Username == request.Username, cancellationToken: cancellationToken);
        if (user == null)
            throw new NotFoundException(ErrorMessages.User_NotFound);
    
        if (!HashingHelper.VerifyPasswordHash(request.OldPassword, user!.PasswordHash, user.PasswordSalt))
            throw new BusinessException("Mevcut şifre hatalı!");
    
        HashingHelper.CreatePasswordHash(request.NewPassword, out var passwordHash, out var passwordSalt);
    
        user!.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;
        
        await _unitOfWork.UserRepository.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return user.Id;
    }
}