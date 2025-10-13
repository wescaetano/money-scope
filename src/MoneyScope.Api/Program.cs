using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoneyScope.Api.Extensions;
using MoneyScope.Api.Middlewares;
using MoneyScope.Application.Config;
using MoneyScope.Application.Models.SendEmail;
using MoneyScope.Core.Token;
using MoneyScope.Infra.Context;
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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthenticationConfiguration(config);
builder.Services.AddAuthorizationConfiguration();
builder.Services.AddSwaggerConfiguration();

builder.Services.Configure<SmtpConfig>(options =>
{
    builder.Configuration.GetSection(nameof(SmtpConfig)).Bind(options);
});

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
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SantoAndre v1");
    c.InjectStylesheet("/swagger-ui/swagger-dark.css");
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






//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//var summaries = new[]
//{
//    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//};

//app.MapGet("/weatherforecast", () =>
//{
//    var forecast =  Enumerable.Range(1, 5).Select(index =>
//        new WeatherForecast
//        (
//            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//            Random.Shared.Next(-20, 55),
//            summaries[Random.Shared.Next(summaries.Length)]
//        ))
//        .ToArray();
//    return forecast;
//})
//.WithName("GetWeatherForecast")
//.WithOpenApi();

//app.Run();

//record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
//{
//    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
//}
