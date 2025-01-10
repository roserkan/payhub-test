namespace Payhub.Application.Common.DTOs.Users;

public class TwoFactorDto
{
    public string Username { get; set; }
    public string Code { get; set; } = null!;
}