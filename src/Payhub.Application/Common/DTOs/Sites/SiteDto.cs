namespace Payhub.Application.Common.DTOs.Sites;

public sealed record SiteDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string InfraName { get; set; } = null!;
    public string Address { get; set; } = null!;
    public IEnumerable<SpwDto> SitePaymentWays { get; set; } = new List<SpwDto>();
}