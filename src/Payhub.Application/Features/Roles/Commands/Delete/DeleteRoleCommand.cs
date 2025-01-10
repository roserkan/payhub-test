using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Roles.Commands.Delete;

public sealed record DeleteRoleCommand : ICommand<int>
{
    public int Id { get; set; }
    public DeleteRoleCommand(int id)
    {
        Id = id;
    }
}