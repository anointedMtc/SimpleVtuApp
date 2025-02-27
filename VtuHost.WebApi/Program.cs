using Asp.Versioning.ApiExplorer;
using ExternalServices.Api;
using Identity.Api;
using Identity.Infrastructure;
using Notification.Api;
using SagaOrchestrationStateMachines;
using Serilog;
using Serilog.Formatting.Compact;
using SharedKernel.Api;
using VtuApp.Api;
using VtuHost.WebApi.Constants;
using VtuHost.WebApi.Extensions;
using VtuHost.WebApi.Middlewares;
using Wallet.Api;

// this will get replaced by the one registered in our IoC container

// METHOD ONE
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("MassTransit", Serilog.Events.LogEventLevel.Debug)
    .Enrich.FromLogContext()
    //.WriteTo.Console()
    .WriteTo.Console(new RenderedCompactJsonFormatter())
    .WriteTo.Seq("http://localhost:5341")
    .WriteTo.File(new CompactJsonFormatter(), "/logs/logformat2-.json", rollingInterval: RollingInterval.Day)
    //.CreateLogger();
    .CreateBootstrapLogger(); // use this especially when you want to override with the one below

// METHOD TWO
//var configuration = new ConfigurationBuilder()
//    .SetBasePath(Directory.GetCurrentDirectory())
//    .AddJsonFile("appsettings.json")
//    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true, true)
//    .Build();

//var logger = new LoggerConfiguration()
//    .ReadFrom.Configuration(configuration)
//    .CreateLogger();

Log.Information("Starting web application...");

try
{

    var builder = WebApplication.CreateBuilder(args);

    builder.AddExternalServicesModule();
    builder.AddIdentityModule();
    builder.AddNotificationModule();
    builder.AddVtuAppModule();
    builder.AddWalletsModule();
    builder.AddSagaStateMachinesModule(); 
    builder.AddSharedKernelModule();

    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();

    builder.Services.AddControllers();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

    builder.Host.UseSerilog((context, configuration) =>
       configuration.ReadFrom.Configuration(context.Configuration));

    builder.Services.AddHttpContextAccessor();

    builder.Services.ConfigureApiVersioning();
    builder.Services.ConfigureCorsPolicy();
    builder.Services.ConfigureHangfireBackgroundService(builder.Configuration);
    builder.Services.ConfigureHealthChecks(builder.Configuration);
    builder.Services.ConfigureMassTransitServices(builder.Configuration);
    builder.Services.ConfigureRateLimiterServices();
    builder.Services.ConfigureRedisCache(builder.Configuration);
    
    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
            }

            options.DisplayRequestDuration();
        });
    }

    app.UseExceptionHandler(opt => { });

    app.UseMiddleware<RequestLogContextMiddleware>();
    app.UseSerilogRequestLogging();   

    app.UseHttpsRedirection();

    app.UseRateLimiter();

    app.UseCors(CorsPolicyNames.AllowAnyOrigin);

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();


    app.MapHangfireEndpoints();
    app.MapHealthChecksEndpoints();


    app.CreateIdentityDbAndApplyMigrations();
    
    // we don't need this... instead, trigger UserCreatedSagaEvent in the identity Module using the email of users seeded in Identity so that the correct ApplicationUserId would be used for every module
    //app.CreateWalletDbAndApplyMigrations();  

    app.Run();

}
//catch (Exception ex)
// Yes this is expected, you should be able to catch and ignore HostAbortedException in your application code if you're catching and logging all startup exceptions.
catch (Exception ex) when (ex is not HostAbortedException && ex.Source != "Microsoft.EntityFrameworkCore.Design") // see https://github.com/dotnet/efcore/issues/29923
{

    Log.Fatal(ex, "Host terminated unexpectedly. Application startup failed.");

}
finally
{
    Log.CloseAndFlush();
}


