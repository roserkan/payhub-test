using System.Globalization;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.OpenApi.Models;
using Payhub.Application;
using Payhub.Application.Common.Services.BackgroundServices;
using Payhub.Infrastructure;
using Serilog;
using Serilog.Events;
using Shared.Abstractions.Hubs;
using Shared.CrossCuttingConcerns.Exceptions.Extensions;
using Shared.Utils.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", b =>
    {
        b.AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed((host) => true)
            .AllowCredentials();
    });
});

builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddHttpClient();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddSecurityServices(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(opt =>
{
    opt.AddSecurityDefinition(
        name: "Bearer",
        securityScheme: new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description =
                "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345.54321\""
        }
    );
    opt.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                },
                new string[] { }
            }
        }
    );
    
});

// Serilog'u yapılandır
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information() // Minimum seviyeyi belirler
    .Enrich.FromLogContext() // LogContext'i kullanabilir hale getirir
    //.WriteTo.Console(outputTemplate: "[Serilog] {Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}")
    .WriteTo.Async(a => a.Seq(
        "http://52.59.229.12:5341", 
        restrictedToMinimumLevel: LogEventLevel.Information, 
        bufferBaseFilename: "./logs/seq-buffer"))
    .Filter.ByIncludingOnly(logEvent =>
        logEvent.Properties.ContainsKey("GeneralLog") || 
        logEvent.Properties.ContainsKey("PayoxLog") ||
        logEvent.Properties.ContainsKey("InfraLog") || 
        logEvent.Properties.ContainsKey("ConsumerLog")) // Belirli property'leri içeren logları gönder
    .CreateLogger();


builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

// Log.Information("This is a test log from Serilog!");
// Log.Debug("Debug level log.");
// Log.Error("An error occurred.");

Serilog.Debugging.SelfLog.Enable(msg =>
{
    File.AppendAllText("selflog.txt", msg + Environment.NewLine);
});

// Hangfire'ı PostgreSQL ile konfigüre et
builder.Services.AddHangfire(config =>
    config.UsePostgreSqlStorage(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddHangfireServer();
builder.Services.AddScoped<HavaleBotTriggerJob>();

//builder.Host.UseSerilog(); // UseSerilog ile yapılandırma
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.ConfigureCustomExceptionMiddleware();

app.UseHangfireDashboard("/hangfire");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.MapHub<NotificationHub>("/notificationHub");


// Hangfire RecurringJob yapılandırması
using (var scope = app.Services.CreateScope())
{
    var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
    var job = scope.ServiceProvider.GetRequiredService<HavaleBotTriggerJob>();

    recurringJobManager.AddOrUpdate(
        "HavBot Trigger4",
        () => job.Execute(),
        Cron.Daily(23, 59)
    );
}
//   => "*/30 * * * * *" => Her 30 saniyede bir çalıştır
app.Run();
