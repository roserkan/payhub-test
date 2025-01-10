namespace Payhub.Application.Common.DTOs.Analysis;

public class SiteAnalysResponseDto
{
    public List<SiteAnalysDto> SiteAnalysis { get; set; } = new List<SiteAnalysDto>();
    public SiteAnalysAllDto SiteAnalysisAll  { get; set; } = new SiteAnalysAllDto();
}