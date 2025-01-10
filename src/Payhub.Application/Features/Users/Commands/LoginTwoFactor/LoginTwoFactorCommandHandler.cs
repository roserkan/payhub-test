using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.Constants;
using Payhub.Application.Common.DTOs.Users;
using Payhub.Domain.Enums;
using Shared.Abstractions.Messaging;
using Shared.CrossCuttingConcerns.Exceptions.Types;
using Shared.Utils.Security.Hashing;
using Shared.Utils.Security.JWT;

namespace Payhub.Application.Features.Users.Commands.LoginTwoFactor;

public sealed class LoginTwoFactorCommandHandler : ICommandHandler<LoginTwoFactorCommand, VerifiedLoggedDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenHelper _tokenHelper;
    
    public LoginTwoFactorCommandHandler(IUnitOfWork unitOfWork, ITokenHelper tokenHelper)
    {
        _unitOfWork = unitOfWork;
        _tokenHelper = tokenHelper;
    }
    
    public async Task<VerifiedLoggedDto> Handle(LoginTwoFactorCommand request, CancellationToken cancellationToken)
    {
        // var loggedUser = await _unitOfWork.UserRepository.GetWithSelectorAsync<LoggedDto>(
        //     predicate: i => i.Username == request.Username,
        //     selector: u => new VerifiedLoggedDto
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
        //         IsTwoFactorEnabled = u.IsTwoFactorEnabled   
        //     });
        
        var verifiedLoggedUser = await _unitOfWork.UserRepository.GetWithSelectorAsync(i => i.Username == request.Username, cancellationToken: cancellationToken,
            selector: u => new VerifiedLoggedDto
            {
                Id = u.Id,
                Name = u.Name,
                Username = u.Username, 
                Roles = u.UserRoles.Select(x => x.Role.Name).ToList(),
                RoleType = u.UserRoles.Select(x => x.Role.RoleType).FirstOrDefault() ?? RoleType.None,
                SystemPermissions = u.UserRoles
                             .SelectMany(ur => ur.Role.RoleSystemPermissions)
                             .Select(rp => rp.SystemPermission.Key)
                             .ToList(),
                SiteIds = u.UserRoles
                             .SelectMany(ur => ur.Role.RoleSitePermissions)
                             .Select(rsp => rsp.SiteId)
                             .ToList(),
            });
        
        if (verifiedLoggedUser == null)
            throw new BusinessException(ErrorMessages.User_NotFound);
        
        
        AccessToken createdAccessToken = _tokenHelper.CreateToken(verifiedLoggedUser.Id,
            verifiedLoggedUser.Roles?.ToArray(), 
            verifiedLoggedUser.SystemPermissions?.ToArray(), 
            verifiedLoggedUser.SiteIds?.ToArray(),
            verifiedLoggedUser.AffiliateIds?.ToArray(),
            (int)verifiedLoggedUser.RoleType);
        
        verifiedLoggedUser.AccessToken = createdAccessToken;
        return verifiedLoggedUser;
    }
}