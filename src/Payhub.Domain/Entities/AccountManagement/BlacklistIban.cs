using Shared.Domain;

namespace Payhub.Domain.Entities.AccountManagement;

public class BlacklistIban : BaseEntity
{
    public string Iban { get; set; } = null!;
}