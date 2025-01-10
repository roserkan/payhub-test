namespace Payhub.Application.Common.DTOs.BlaklistIbans;

public sealed record BlacklistIbanDto
{
    public int Id { get; set; }
    public string Iban { get; set; } = null!;
}