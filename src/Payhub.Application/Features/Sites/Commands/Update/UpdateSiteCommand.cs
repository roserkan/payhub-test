using Payhub.Application.Common.Constants;
using Payhub.Application.Common.DTOs.Sites;

using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Sites.Commands.Update;

public sealed record UpdateSiteCommand : ICommand<int>
{
    public int Id { get; set; }
    public int InfrastructureId { get; set; }
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public List<UpdateSpwDto> SitePaymentWays { get; set; } = new();
    
    // Cache Remove
    public bool BypassCache { get; set; } = false;
    public IEnumerable<string> KeyPatternList { get; set; } = CacheKeys.Site_Remove;
}