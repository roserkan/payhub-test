using Payhub.Application.Common.DTOs.Users;

using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Users.Queries.GetAll;

public sealed record GetAllUsersQuery : IQuery<IEnumerable<UserDto>>
{
}