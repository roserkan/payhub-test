namespace Payhub.Application.Common.DTOs.Analysis;

public class AffiliateAnalysAllDto
{
    public decimal TotalDepositAmount { get; set; }
    public int TotalDepositCount { get; set; }
    public decimal TotalWithdrawAmount { get; set; }
    public int TotalWithdrawCount { get; set; }
    public decimal TotalCommission { get; set; }
    public decimal AffiliateTotalCommission { get; set; }
    public decimal TotalBalance { get; set; }
}