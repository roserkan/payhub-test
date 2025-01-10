namespace Payhub.Application.Common.DTOs.Users;

public sealed record LoggedDto
{
    public string Username { get; set; } = null!;
}