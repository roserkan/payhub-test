using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.DTOs.Users;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Users.Queries.GetAll;

public sealed class GetAllUsersQueryHandler : IQueryHandler<GetAllUsersQuery, IEnumerable<UserDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public GetAllUsersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<IEnumerable<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _unitOfWork.UserRepository.GetAllWithSelectorAsync<UserDto>(
            predicate: i => !i.IsDeleted,
            selector: i => new UserDto
            {
                Id = i.Id,
                Name = i.Name,
                Username = i.Username,
                RoleName = i.UserRoles.Select(x => x.Role.Name).FirstOrDefault() ?? "---",
                RoleId = i.UserRoles.Select(x => x.RoleId).FirstOrDefault(),
                IsTwoFactorEnabled = i.IsTwoFactorEnabled,
                TwoFactorSecret = i.TwoFactorSecret,
                FirstPassword = i.FirstPassword
            },
            cancellationToken: cancellationToken);
        
        return users;
    }
}