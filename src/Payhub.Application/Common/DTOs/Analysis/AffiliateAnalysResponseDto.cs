namespace Payhub.Application.Common.DTOs.Analysis;

public class AffiliateAnalysResponseDto
{
    public List<AffiliateAnalysDto> AffiliateAnalysis { get; set; } = new List<AffiliateAnalysDto>();
    public AffiliateAnalysAllDto AffiliateAnalysisAll  { get; set; } = new AffiliateAnalysAllDto();
}