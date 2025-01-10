namespace Payhub.Application.Common.DTOs.Accounts;

public sealed record BankDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string IconUrl { get; set; } = string.Empty;
}