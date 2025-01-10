using Microsoft.AspNetCore.Http;
using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.Constants;
using Payhub.Application.Common.DTOs.Users;
using Shared.Abstractions.Messaging;
using Shared.CrossCuttingConcerns.Exceptions.Types;
using Shared.Utils.Security.Extensions;

namespace Payhub.Application.Features.Users.Queries.CurrentUser;

public sealed class GetCurrentUserQueryHandler : IQueryHandler<GetCurrentUserQuery, CurrentUserDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public GetCurrentUserQueryHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<CurrentUserDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var id = _httpContextAccessor.HttpContext?.User.GetUserId();
        if (id == null)
            throw new NotFoundException(ErrorMessages.User_NotFound);
        
        var user = await _unitOfWork.UserRepository.GetWithSelectorAsync<CurrentUserDto>(
            predicate: i => i.Id == id,
            selector: u => new CurrentUserDto
            {
                Id = u.Id,
                Name = u.Name,
                Username = u.Username,
                Roles = u.UserRoles.Select(x => x.Role.Name).ToList(),
                RoleType = u.UserRoles.Select(x => x.Role.RoleType).FirstOrDefault(),
                Permissions = u.UserRoles
                    .SelectMany(ur => ur.Role.RoleSystemPermissions)
                    .Select(rp => rp.SystemPermission.Key)
                    .ToList(),
                Affiliates = u.UserRoles.SelectMany(x => x.Role.RoleAffiliatePermissions).Select(x => x.AffiliateId).ToList()
            }, cancellationToken: cancellationToken);
        
        if (user == null)
            throw new NotFoundException(ErrorMessages.User_NotFound);
        
        return user;
    }
}