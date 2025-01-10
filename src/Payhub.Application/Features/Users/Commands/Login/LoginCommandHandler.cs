using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.Constants;
using Payhub.Application.Common.DTOs.Users;
using Shared.Abstractions.Messaging;
using Shared.CrossCuttingConcerns.Exceptions.Types;
using Shared.Utils.Security.Hashing;
using Shared.Utils.Security.JWT;

namespace Payhub.Application.Features.Users.Commands.Login;

public sealed class LoginCommandHandler : ICommandHandler<LoginCommand, LoggedDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenHelper _tokenHelper;
    
    public LoginCommandHandler(IUnitOfWork unitOfWork, ITokenHelper tokenHelper)
    {
        _unitOfWork = unitOfWork;
        _tokenHelper = tokenHelper;
    }
    
    public async Task<LoggedDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // LoggedDto? loggedUser = await _unitOfWork.UserRepository.GetWithSelectorAsync<LoggedDto>(
        //     predicate: i => i.Username == request.Username,
        //     selector: u => new LoggedDto()
        //     {
        //         Id = u.Id,
        //         Name = u.Name, 
        //         Username = u.Username,
        //         Roles = u.UserRoles.Select(x => x.Role.Name).ToList(),
        //         SystemPermissions = u.UserRoles
        //             .SelectMany(ur => ur.Role.RoleSystemPermissions)
        //             .Select(rp => rp.SystemPermission.Key)
        //             .ToList(),
        //         SiteIds = u.UserRoles
        //             .SelectMany(ur => ur.Role.RoleSitePermissions)
        //             .Select(rsp => rsp.SiteId)
        //             .ToList(),
        //         AffiliateIds = u.UserRoles 
        //             .SelectMany(ur => ur.Role.RoleAffiliatePermissions)
        //             .Select(rap => rap.AffiliateId)
        //             .ToList(),
        //         IsTwoFactorEnabled = u.IsTwoFactorEnabled,
        //         PasswordHash = u.PasswordHash,
        //         PasswordSalt = u.PasswordSalt
        //     },
        //     cancellationToken: cancellationToken);

        var loggedUser = await _unitOfWork.UserRepository.GetAsync(i => i.Username == request.Username,
            cancellationToken: cancellationToken);
        
        if (loggedUser is null)
            throw new BusinessException(ErrorMessages.User_NotFound);
         
        if (!HashingHelper.VerifyPasswordHash(request.Password, loggedUser.PasswordHash, loggedUser.PasswordSalt))
            throw new BusinessException(ErrorMessages.User_InvalidLoginCredentials);
        
        return new LoggedDto
        {
            Username = loggedUser.Username,
        };
    }
}