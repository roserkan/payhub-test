using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Payhub.Domain.Entities;
using Payhub.Domain.Entities.AccountManagement;
using Payhub.Domain.Entities.AffiliateManagement;
using Payhub.Domain.Entities.BotManagement;
using Payhub.Domain.Entities.CustomerManagement;
using Payhub.Domain.Entities.PaymentWayManagement;
using Payhub.Domain.Entities.SafeManagement;
using Payhub.Domain.Entities.SiteManagement;
using Payhub.Domain.Entities.TransactionManagement;
using Payhub.Domain.Entities.UserManagement;

namespace Payhub.Infrastructure.Persistence.Contexts;

public class ApplicationDbContext : DbContext
{
    // Identity management
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<SystemPermission> SystemPermissions { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<RoleSystemPermission> RoleSystemPermission { get; set; }
    public DbSet<RoleSitePermission> RoleSitePermissions { get; set; }
    public DbSet<RoleAffiliatePermission> RoleAffiliatePermissions { get; set; }
    
    // Transaction management
    public DbSet<Deposit> Deposits { get; set; }
    public DbSet<Withdraw> Withdraws { get; set; }

    // Site management
    public DbSet<Site> Site { get; set; }
    public DbSet<SitePaymentWay> SitePaymentWays { get; set; }
    public DbSet<Domain.Entities.SiteManagement.Infrastructure> Infrastructures { get; set; }
    
    // PaymentWay management
    public DbSet<PaymentWay> PaymentWays { get; set; }
    
    // Customer management
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Blacklist> Blacklists { get; set; }
    
    // Account management
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Bank> Banks { get; set; }
    public DbSet<AccountSite> AccountSites { get; set; }
    
    // Affiliate management
    public DbSet<Affiliate> Affiliates { get; set; }
    public DbSet<AffiliateSite> AffiliateSites { get; set; }
    
    // Safe management
    public DbSet<SiteSafeMove> SiteSafeMoves { get; set; }
    public DbSet<AffiliateSafeMove> AffiliateSafeMoves { get; set; }
    
    public DbSet<BlacklistIban> BlacklistIbans { get; set; }
    
    // Bot Management
    public DbSet<Device> Devices { get; set; }
    public DbSet<WithdrawOrder> WithdrawOrders { get; set; }
    public DbSet<HavaleBotMove> HavaleBotMoves { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions)
        : base(dbContextOptions)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}