using Shared.Domain;

namespace Payhub.Domain.Entities.AccountManagement;

public class Bank : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string IconUrl { get; set; } = string.Empty;
}