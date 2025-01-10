using Payhub.Application.Common.DTOs.Sites;

using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Sites.Commands.Create;

public sealed record CreateSiteCommand : ICommand<int>
{
    public int InfrastructureId { get; set; }
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public List<CreateSpwDto> SitePaymentWays { get; set; } = new();
}