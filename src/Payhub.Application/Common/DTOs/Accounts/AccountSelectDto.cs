using Shared.Utils.Responses;

namespace Payhub.Application.Common.DTOs.Accounts;

public sealed class AccountSelectDto : SelectDto
{
    public string? AccountNumber { get; set; }
    public string BankName { get; set; } = null!;
    public string BankIconUrl { get; set; } = null!;
}