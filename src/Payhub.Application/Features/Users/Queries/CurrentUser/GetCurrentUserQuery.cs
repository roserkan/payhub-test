using Payhub.Application.Common.DTOs.Users;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Users.Queries.CurrentUser;

public sealed record GetCurrentUserQuery : IQuery<CurrentUserDto>
{
}