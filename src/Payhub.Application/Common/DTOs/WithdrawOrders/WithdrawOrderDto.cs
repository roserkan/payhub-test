using Payhub.Application.Common.DTOs.Accounts;
using Payhub.Domain.Enums;
using Shared.Utils.Requests;

namespace Payhub.Application.Common.DTOs.WithdrawOrders;

public sealed class WithdrawOrderDto
{
    public int Id { get; set; }
    public AccountDto SenderAccount { get; set; } = null!;
    public string ReceiverAccountNumber { get; set; } = null!;
    public string ReceiverFullName { get; set; } = null!;
    public decimal Amount { get; set; }
    public WithdrawOrderStatus Status { get; set; }
    public string? Description { get; set; }
    public DateTime TransactionDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}

public sealed class WithdrawOrderFilterDto : DateFilterDto
{
}