using System.Reflection;
using Hangfire;
using Hangfire.PostgreSql;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Payhub.Application.Abstractions.Services;
using Payhub.Application.Common.Pipelines.Logging;
using Payhub.Application.Common.Services;
using Payhub.Application.Common.Services.BackgroundServices;
using Payhub.Application.Features.Affiliates.DynamicAffiliates.Payox;
using Payhub.Application.Features.Affiliates.DynamicAffiliates.Payox.Services;
using Payhub.Application.Features.Deposits.EventConsumers;
using Payhub.Application.Features.Withdraws.EventConsumers;

namespace Payhub.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(conf =>
        {
            conf.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });        
        
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        services.AddMassTransit(x =>
        {
            x.AddConsumer<CreatedDepositEventConsumer>();
            x.AddConsumer<CreatedWithdrawEventConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration["RabbitMQ:HostName"], h =>
                {
                    h.Username(configuration["RabbitMQ:UserName"]!);
                    h.Password(configuration["RabbitMQ:Password"]!);
                });

                cfg.ReceiveEndpoint(configuration["RabbitMQ:DepositQueue"]!, e =>
                {
                    e.ConfigureConsumer<CreatedDepositEventConsumer>(context);
                    e.UseMessageRetry(r =>
                    {
                        r.Interval(3, TimeSpan.FromSeconds(5)); // İlk deneme
                        r.Immediate(2); // Anında iki kez daha dene
                    });
                    e.UseInMemoryOutbox(); // Mesaj tekrar işlenene kadar kuyrukta tutulur.
                    e.PrefetchCount = 50; // Performans kontrolü için.
                });
                
                cfg.ReceiveEndpoint(configuration["RabbitMQ:WithdrawQueue"]!, e =>
                {
                    e.ConfigureConsumer<CreatedWithdrawEventConsumer>(context);
                    e.UseMessageRetry(r =>
                    {
                        r.Interval(3, TimeSpan.FromSeconds(5)); // İlk deneme
                        r.Immediate(2); // Anında iki kez daha dene
                    });
                    e.UseInMemoryOutbox(); // Mesaj tekrar işlenene kadar kuyrukta tutulur.
                    e.PrefetchCount = 50; // Performans kontrolü için.
                });
            });
        });

        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IInfraService, InfraService>();
        services.AddScoped<ITransactionStatusService, TransactionStatusService>();
        services.AddScoped<IPayoxService, PayoxService>();
        
        return services;
    }
}