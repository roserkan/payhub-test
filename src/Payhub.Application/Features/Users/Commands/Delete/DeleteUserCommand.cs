using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Users.Commands.Delete;

public sealed record DeleteUserCommand : ICommand<int>
{
    public int Id { get; set; }
    public DeleteUserCommand(int id)
    {
        Id = id;
    }
}