using Payhub.Domain.Enums;
using Shared.Domain;

namespace Payhub.Domain.Entities.CustomerManagement;

public class Blacklist : BaseEntity
{
    public BlacklistType BlacklistType { get; set; } // Kara listeleme türü
    public string Value { get; set; } = string.Empty;
}