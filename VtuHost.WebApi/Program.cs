using Asp.Versioning.ApiExplorer;
using Elastic.Channels;
using Elastic.Ingest.Elasticsearch;
using Elastic.Ingest.Elasticsearch.DataStreams;
using Elastic.Serilog.Sinks;
using ExternalServices.Api;
using Identity.Api;
using Identity.Infrastructure;
using Notification.Api;
using SagaOrchestrationStateMachines;
using Serilog;
using Serilog.Exceptions;
using Serilog.Formatting.Compact;
using SharedKernel.Api;
using System.Reflection;
using VtuApp.Api;
using VtuHost.WebApi.Constants;
using VtuHost.WebApi.Extensions;
using VtuHost.WebApi.Middlewares;
using Wallet.Api;

// Reading data from configuration file instead of writing it raw like we did for Seq
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", false, true)
    .Build();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("MassTransit", Serilog.Events.LogEventLevel.Debug)
    .Enrich.FromLogContext()
    .Enrich.WithExceptionDetails()
    .WriteTo.Console(new RenderedCompactJsonFormatter())
    .WriteTo.Debug(new RenderedCompactJsonFormatter())
    .WriteTo.Seq("http://localhost:5341")
    .WriteTo.File(new CompactJsonFormatter(), $"{Directory.GetCurrentDirectory()}/logs/logformat2-.json", rollingInterval: RollingInterval.Day)
    .WriteTo.Elasticsearch([new Uri(configuration["ElasticConfiguration:Uri"]!)], opts =>
    {
        opts.DataStream = new DataStreamName("logs", "simple-Vtu-App", "vtuApp");
        opts.BootstrapMethod = BootstrapMethod.Failure;
        opts.ConfigureChannel = channelOpts =>
        {
            channelOpts.BufferOptions = new BufferOptions();
        };
        
    }, transport =>
    {
        // transport.Authentication(new BasicAuthentication(username, password)); // Basic Auth
        // transport.Authentication(new ApiKey(base64EncodedApiKey)); // ApiKey
    })
    .CreateBootstrapLogger(); 


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
catch (Exception ex) when (ex is not HostAbortedException && ex.Source != "Microsoft.EntityFrameworkCore.Design") 
{

    Log.Fatal(ex, "Host terminated unexpectedly. Application startup failed.");

}
finally
{
    Log.CloseAndFlush();
}



//void ConfigureLogging()
//{
//    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

//    var configuration = new ConfigurationBuilder()
//        .AddJsonFile("appsettings.json", false, true)
//        .AddJsonFile($"appsettings.{environment}.json", false)
//        .Build();

//    Log.Logger = new LoggerConfiguration()
//        .Enrich.FromLogContext()
//        .Enrich.WithExceptionDetails()
//        .WriteTo.Debug()
//        .WriteTo.Console()
//        .WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment))
//        .Enrich.WithProperty("Environment", environment)
//        .ReadFrom.Configuration(configuration)
//        .CreateLogger();
//}

//ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration, string environment)
//{
//    return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]))
//    {
//        AutoRegisterTemplate = true,
//        IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-")}-{environment.ToLower()}-{DateTimeOffset.UtcNow:yyyy-MM}"
//        NumberOfReplicas = 1,
//        NumberOfShards = 2
//    };
//}