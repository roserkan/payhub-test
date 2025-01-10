namespace Payhub.Application.Common.DTOs.Analysis;

public class AffiliateAnalysDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public decimal DepositAmount { get; set; }
    public int DepositCount { get; set; }
    public decimal WithdrawAmount { get; set; }
    public int WithdrawCount { get; set; }
    public decimal CommissionAmount { get; set; }
    public decimal AffiliateCommissionAmount { get; set; }

    public decimal ExternalInAmount { get; set; }
    public decimal ExternalOutAmount { get; set; }
    public decimal ExternalCommissionAmount { get; set; }

    public decimal Balance { get; set; }
}