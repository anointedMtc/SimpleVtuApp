using Asp.Versioning.ApiExplorer;
using ExternalServices.Api;
using Identity.Api;
using Identity.Infrastructure;
using Notification.Api;
using SagaOrchestrationStateMachines;
using Serilog;
using SharedKernel.Api;
using VtuApp.Api;
using VtuHost.WebApi.Constants;
using VtuHost.WebApi.Extensions;
using VtuHost.WebApi.Middlewares;
using Wallet.Api;

try
{
    Log.Information("Application Stating Up");

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
catch (Exception ex)
{

    Log.Fatal(ex, "Application startup failed");

}
finally
{
    Log.CloseAndFlush();
}


