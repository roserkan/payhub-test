using Payhub.Application.Common.DTOs.Accounts;

namespace Payhub.Application.Common.DTOs.Devices;

public sealed class DeviceDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; } 
    public string SerialNumber { get; set; } = null!;
    public int? AccountId { get; set; }
    public AccountDto Account { get; set; } = new AccountDto();
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}