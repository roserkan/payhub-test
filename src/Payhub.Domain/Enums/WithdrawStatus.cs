namespace Payhub.Domain.Enums;

public enum WithdrawStatus
{
    PendingWithdraw = 0,
    Confirmed = 1,
    Declined = 2,
    TimeOut = 3
}