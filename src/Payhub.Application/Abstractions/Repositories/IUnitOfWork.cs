namespace Payhub.Application.Abstractions.Repositories;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    IRoleRepository RoleRepository { get; }
    IAccountRepository AccountRepository { get; }
    IInfrastructureRepository InfrastructureRepository { get; }
    ISiteRepository SiteRepository { get; }
    IBlacklistRepository BlacklistRepository { get; }
    IDepositRepository DepositRepository { get; }
    ICustomerRepository CustomerRepository { get; }
    IPaymentWayRepository PaymentWayRepository { get; }
    IAffiliateRepository AffiliateRepository { get; }
    ISystemPermissionRepository SystemPermissionRepository { get; }
    IBankRepository BankRepository { get; }
    IWithdrawRepository WithdrawRepository { get; }
    ISiteSafeMoveRepository SiteSafeMoveRepository { get; }
    IAffiliateSafeMoveRepository AffiliateSafeMoveRepository { get; }
    IBlacklistIbanRepository BlacklistIbanRepository { get; }
    IDeviceRepository Devices { get; }
    IWithdrawOrderRepository WithdrawOrders { get; }
    IHavaleBotMoveRepository HavaleBotMoves { get; }
    
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    void Rollback();
}