using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.SiteSafeMoves.Commands.Delete;

public sealed record DeleteSiteSafeMoveCommand : ICommand<int>
{
    public int Id { get; set; }
    
    public DeleteSiteSafeMoveCommand(int id)
    {
        Id = id;
    }
}