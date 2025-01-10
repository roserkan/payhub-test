using Shared.Domain;

namespace Payhub.Domain.Entities.SiteManagement;

public class Infrastructure : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string DepositAddress { get; set; } = string.Empty;
    public string WithdrawAddress { get; set; } = string.Empty;
}