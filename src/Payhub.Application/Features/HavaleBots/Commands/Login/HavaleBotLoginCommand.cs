using MediatR;
using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.Constants;
using Payhub.Application.Common.DTOs.Users;
using Payhub.Application.Features.HavaleBots.BotResults;
using Payhub.Domain.Enums;
using Shared.Abstractions.Messaging;
using Shared.CrossCuttingConcerns.Exceptions.Types;
using Shared.Utils.Security.JWT;

namespace Payhub.Application.Features.HavaleBots.Commands.Login;

public sealed record HavaleBotLoginCommand(string Username, string Password) : ICommand<BotResult<AccessToken>>;


public sealed class HavaleBotLoginCommandHandler : ICommandHandler<HavaleBotLoginCommand, BotResult<AccessToken>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenHelper _tokenHelper;
    
    public HavaleBotLoginCommandHandler(IUnitOfWork unitOfWork, ITokenHelper tokenHelper)
    {
        _unitOfWork = unitOfWork;
        _tokenHelper = tokenHelper;
    }
    
    public async Task<BotResult<AccessToken>> Handle(HavaleBotLoginCommand request, CancellationToken cancellationToken)
    {
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
            (int)verifiedLoggedUser.RoleType
            );
        
        return BotResult<AccessToken>.Ok(createdAccessToken);
    }
}
