namespace Payhub.Domain.Enums;

public enum DepositStatus
{
    PendingDeposit = 0,
    PendingConfirmation = 1,
    Confirmed = 2,
    Declined = 3,
    TimeOut = 4,
}