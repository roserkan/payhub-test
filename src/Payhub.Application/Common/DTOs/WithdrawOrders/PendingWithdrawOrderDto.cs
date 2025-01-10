namespace Payhub.Application.Common.DTOs.WithdrawOrders;

public sealed class PendingWithdrawOrderDto
{
    public int Id { get; set; }
    public string ReceiverAccountNumber { get; set; } = string.Empty;
    public string ReceiverAccountFullName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public int SenderAccountId { get; set; }
    public bool IsCompleted { get; set; }
    public string BankName {  get; set; } = string.Empty;
}