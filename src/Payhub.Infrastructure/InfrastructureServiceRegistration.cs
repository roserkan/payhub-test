using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Payhub.Application.Abstractions.Repositories;
using Payhub.Infrastructure.Persistence.Contexts;
using Payhub.Infrastructure.Persistence.Repositories;
using Payhub.Infrastructure.Persistence.Repositories.Base;

namespace Payhub.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var cs = configuration.GetConnectionString("Default");
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(cs);
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IInfrastructureRepository, InfrastructureRepository>();
        services.AddScoped<ISiteRepository, SiteRepository>();
        services.AddScoped<IBlacklistRepository, BlacklistRepository>();
        services.AddScoped<IDepositRepository, DepositRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IPaymentWayRepository, PaymentWayRepository>();
        services.AddScoped<IAffiliateRepository, AffiliateRepository>();
        services.AddScoped<ISystemPermissionRepository, SystemPermissionRepository>();
        services.AddScoped<IBankRepository, BankRepository>();
        services.AddScoped<IWithdrawRepository, WithdrawRepository>();
        services.AddScoped<ISiteSafeMoveRepository, SiteSafeMoveRepository>();
        services.AddScoped<IAffiliateSafeMoveRepository, AffiliateSafeMoveRepository>();
        services.AddScoped<IBlacklistIbanRepository, BlacklistIbanRepository>();
        services.AddScoped<IDeviceRepository, DeviceRepository>();
        services.AddScoped<IWithdrawOrderRepository, WithdrawOrderRepository>();
        services.AddScoped<IHavaleBotMoveRepository, HavaleBotMoveRepository>();
        
        return services;
    }
}