using Payhub.Domain.Enums;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Blacklists.Commands.Delete;

public sealed record DeleteBlacklistCommand : ICommand<int>
{
    public string PanelCustomerId { get; set; }
    
    public DeleteBlacklistCommand(string panelCustomerId)
    {
        PanelCustomerId = panelCustomerId;
    }
}