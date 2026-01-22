using Application.Services;
using Hangfire;
using Hangfire.Console;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoneyScope.Api.Extensions;
using MoneyScope.Api.Middlewares;
using MoneyScope.Application.Config;
using MoneyScope.Application.Interfaces;
using MoneyScope.Application.Models.SendEmail;
using MoneyScope.Application.Services;
using MoneyScope.Core.Token;
using MoneyScope.Infra.Context;
using MoneyScope.Infra.Interfaces;
using MoneyScope.Ioc;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);
ConfigurationManager config = builder.Configuration;
builder.Services.AddSingleton(d => config);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddDbContext<MoneyScopeContext>(options =>
{
    options.UseMySql(config["ConnectionStrings:Conn"],
        new MySqlServerVersion(new Version(8, 0)))
        .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddFilter((category, level) =>
            category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information)));
}, ServiceLifetime.Transient);


var logoPath = Path.Combine(
    builder.Environment.ContentRootPath,
    "wwwroot",
    "Assets",
    "moneyscope-logo.png"
);

builder.Services.AddScoped<IReportPdfService>(sp =>
{
    var repositoryFactory = sp.GetRequiredService<IRepositoryFactory>();
    var emailService = sp.GetRequiredService<IAuthService>();

    return new ReportPdfService(
        repositoryFactory,
        emailService,
        logoPath
    );
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthenticationConfiguration(config);
builder.Services.AddAuthorizationConfiguration();
builder.Services.AddSwaggerConfiguration();

builder.Services.AddHangfire(options =>
{
    options.UseMemoryStorage();
    options.UseConsole();
});
builder.Services.AddHangfireServer();
builder.Services.AddHostedService<ServiceBackGround>();

builder.Services.Configure<SmtpConfig>(options =>
{
    builder.Configuration.GetSection(nameof(SmtpConfig)).Bind(options);
});
//configura mapeamentos de IOptions do appsettings
builder.Services.AddConfiguredOptions(builder.Configuration);

builder.Services.Configure<EnvironmentVars>(options =>
{
    builder.Configuration.GetSection("Vars").Bind(options);
});

builder.Services.AddMemoryCache();

var migrationConfig = builder.Configuration.GetSection(nameof(MigrationConfig)).Get<MigrationConfig>();

builder.Services.InjectDependencies(migrationConfig);
builder.Services.AddHttpContextAccessor();

//Cors
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
});

var app = builder.Build();


//swagger
app.UseStaticFiles();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MoneyScope v1");
    c.InjectStylesheet("/swagger-ui/swagger-dark.css");
});

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = HangFireDashboard.AuthAuthorizationFilters()
});


app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

#region Middlewares
app.UseMiddleware<JwtMiddleware>();
app.UseMiddleware(typeof(HandlingMiddleware));
#endregion Middlewares

app.MapControllers();
app.Run();

