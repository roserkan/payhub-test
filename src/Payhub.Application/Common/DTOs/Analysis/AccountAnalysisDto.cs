using Payhub.Application.Common.DTOs.Accounts;

namespace Payhub.Application.Common.DTOs.Analysis;

public class AccountAnalysisDto
{
    public int Id { get; set; }
    public AccountDto Account { get; set; } = new();

    public decimal DepositAmount { get; set; }
    public int DepositCount { get; set; }
    public decimal WithdrawAmount { get; set; }
    public int WithdrawCount { get; set; }
    public decimal CommissionAmount { get; set; }
    public decimal Balance { get; set; }
}