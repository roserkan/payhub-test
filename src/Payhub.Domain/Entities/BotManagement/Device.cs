using Payhub.Domain.Entities.AccountManagement;
using Shared.Domain;

namespace Payhub.Domain.Entities.BotManagement;

public class Device : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; } 
    public string SerialNumber { get; set; } = null!;
    public int? AccountId { get; set; }
    
    // Navigation properties
    public Account Account { get; set; } = null!;
}