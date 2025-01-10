using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.AffiliateSafeMoves.Commands.Delete;

public sealed record DeleteAffiliateSafeMoveCommand : ICommand<int>
{
    public int Id { get; set; }
    
    public DeleteAffiliateSafeMoveCommand(int id)
    {
        Id = id;
    }
}