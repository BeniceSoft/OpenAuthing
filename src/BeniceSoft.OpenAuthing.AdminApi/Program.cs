using System.Text;
using BeniceSoft.OpenAuthing;
using BeniceSoft.OpenAuthing.Configuration.Extensions;
using Serilog;
using Serilog.Events;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();
Log.Logger = new LoggerConfiguration()
#if DEBUG
    .MinimumLevel.Verbose()
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.AspNetCore.Identity", LogEventLevel.Debug)
#else
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
#endif
    .Enrich.FromLogContext()
    .ReadFrom.Configuration(configuration)
    .WriteTo.Console()
    .CreateLogger();

try
{
    Log.Information("Starting web host.");
    var builder = WebApplication.CreateBuilder(args);
    builder.Host
        .AddAppSettingsSecretsJson()
        .AddAppSettingsOverrideJson()
        .UseAutofac()
        .UseSerilog();
    await builder.AddApplicationAsync<AuthingAdminApiModule>();
    var app = builder.Build();
    await app.InitializeApplicationAsync();
    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly!");
}
finally
{
    Log.CloseAndFlush();
}