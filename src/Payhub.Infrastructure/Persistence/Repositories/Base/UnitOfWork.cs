using Microsoft.EntityFrameworkCore;
using Payhub.Application.Abstractions.Repositories;
using Payhub.Infrastructure.Persistence.Contexts;

namespace Payhub.Infrastructure.Persistence.Repositories.Base;

public class UnitOfWork : IUnitOfWork
{
    public UnitOfWork(ApplicationDbContext context,
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IAccountRepository accountRepository,
        IInfrastructureRepository infrastructureRepository,
        ISiteRepository siteRepository,
        IBlacklistRepository blacklistRepository,
        IDepositRepository depositRepository,
        ICustomerRepository customerRepository,
        IPaymentWayRepository paymentWayRepository,
        IAffiliateRepository affiliateRepository,
        ISystemPermissionRepository systemPermissionRepository,
        IBankRepository bankRepository,
        IWithdrawRepository withdrawRepository,
        ISiteSafeMoveRepository siteSafeMoveRepository,
        IAffiliateSafeMoveRepository affiliateSafeMoveRepository,
        IBlacklistIbanRepository blacklistIbanRepository,
        IDeviceRepository deviceRepository,
        IWithdrawOrderRepository withdrawOrderRepository,
        IHavaleBotMoveRepository havaleBotMoveRepository)
    {
        _context = context;
        UserRepository = userRepository;
        RoleRepository = roleRepository;
        AccountRepository = accountRepository;
        InfrastructureRepository = infrastructureRepository;
        SiteRepository = siteRepository;
        BlacklistRepository = blacklistRepository;
        DepositRepository = depositRepository;
        CustomerRepository = customerRepository;
        PaymentWayRepository = paymentWayRepository;
        AffiliateRepository = affiliateRepository;
        SystemPermissionRepository = systemPermissionRepository;
        BankRepository = bankRepository;
        WithdrawRepository = withdrawRepository;
        SiteSafeMoveRepository = siteSafeMoveRepository;
        AffiliateSafeMoveRepository = affiliateSafeMoveRepository;
        BlacklistIbanRepository = blacklistIbanRepository;
        Devices = deviceRepository;
        WithdrawOrders = withdrawOrderRepository;
        HavaleBotMoves = havaleBotMoveRepository;
    }

    private readonly ApplicationDbContext _context;
    public IUserRepository UserRepository { get; }
    public IRoleRepository RoleRepository { get; }
    public IAccountRepository AccountRepository { get; }
    public IInfrastructureRepository InfrastructureRepository { get; }
    public ISiteRepository SiteRepository { get; }
    public IBlacklistRepository BlacklistRepository { get; }
    public IDepositRepository DepositRepository { get; }
    public ICustomerRepository CustomerRepository { get; }
    public IPaymentWayRepository PaymentWayRepository { get; }
    public IAffiliateRepository AffiliateRepository { get; }
    public ISystemPermissionRepository SystemPermissionRepository { get; }
    public IBankRepository BankRepository { get; }
    public IWithdrawRepository WithdrawRepository { get; }
    public ISiteSafeMoveRepository SiteSafeMoveRepository { get; set; }
    public IAffiliateSafeMoveRepository AffiliateSafeMoveRepository { get; set; }
    public IBlacklistIbanRepository BlacklistIbanRepository { get; set; }
    public IDeviceRepository Devices { get; set; }
    public IWithdrawOrderRepository WithdrawOrders { get; set; }
    public IHavaleBotMoveRepository HavaleBotMoves { get; set; }
    
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public void Rollback()
    {
        foreach (var entry in _context.ChangeTracker.Entries())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.State = EntityState.Detached;
                    break;
                case EntityState.Modified:
                    entry.State = EntityState.Unchanged;
                    break;
                case EntityState.Deleted:
                    entry.State = EntityState.Unchanged;
                    break;
            }
        }
    }
}
