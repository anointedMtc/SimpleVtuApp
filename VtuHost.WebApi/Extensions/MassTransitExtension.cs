using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Notification.Application.IntegrationEvents.SagaStateMachines.UserCreatedSagaOrchestrator;
using SagaOrchestrationStateMachines.Infrastructure.Persistence;
using SagaOrchestrationStateMachines.Infrastructure.UserCreatedSagaOrchestrator;
using SagaOrchestrationStateMachines.Infrastructure.VtuAirtimeOrderedSagaOrchestrator;
using SagaOrchestrationStateMachines.Infrastructure.VtuDataOrderedSagaOrchestrator;
using VtuApp.Application.Features.Events.ExternalEvents;
using VtuHost.WebApi.Models;
using Wallet.Application.Features.Events.ExternalEvents;

namespace VtuHost.WebApi.Extensions;

public static class MassTransitExtension
{
    public static void ConfigureMassTransitServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MessageBrokerSettings>(configuration.GetSection("MessageBroker"));
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);

        var dbConnectionString = configuration.GetConnectionString("SagaStateMachinesModuleDb");

        services.AddMassTransit(busconfigurator =>
        {
            busconfigurator.SetKebabCaseEndpointNameFormatter();

            busconfigurator.AddConsumers(
                typeof(NotifyApplicationUserOfWalletCreatedEventConsumer).Assembly,
                typeof(BuyAirtimeForCustomerMessageConsumer).Assembly,
                typeof(CreateNewWalletOwnerMessageConsumer).Assembly
            );

            busconfigurator.AddSagaStateMachine<UserCreatedSagaStateMachine, UserCreatedSagaStateInstance>()
            .EntityFrameworkRepository(r =>
            {
                r.ConcurrencyMode = ConcurrencyMode.Optimistic;

                r.AddDbContext<DbContext, SagaStateMachineDbContext>((provider, builder) =>
                {
                    builder.UseSqlServer(dbConnectionString, m =>
                    {
                        m.MigrationsAssembly(typeof(SagaStateMachineDbContext).Assembly.GetName().Name);
                        m.MigrationsHistoryTable($"__{nameof(SagaStateMachineDbContext)}");
                    });
                });
            });

            busconfigurator.AddSagaStateMachine<VtuAirtimeOrderedSagaStateMachine, VtuAirtimeOrderedSagaStateInstance>()
           .EntityFrameworkRepository(r =>
           {
               r.ConcurrencyMode = ConcurrencyMode.Optimistic;

               r.AddDbContext<DbContext, SagaStateMachineDbContext>((provider, builder) =>
               {
                   builder.UseSqlServer(dbConnectionString, m =>
                   {
                       m.MigrationsAssembly(typeof(SagaStateMachineDbContext).Assembly.GetName().Name);
                       m.MigrationsHistoryTable($"__VtuAirtimeOrderedSagaDbContext");
                   });
               });
           });

            busconfigurator.AddSagaStateMachine<VtuDataOrderedSagaStateMachine, VtuDataOrderedSagaStateInstance>()
           .EntityFrameworkRepository(r =>
           {
               r.ConcurrencyMode = ConcurrencyMode.Optimistic;

               r.AddDbContext<DbContext, SagaStateMachineDbContext>((provider, builder) =>
               {
                   builder.UseSqlServer(dbConnectionString, m =>
                   {
                       m.MigrationsAssembly(typeof(SagaStateMachineDbContext).Assembly.GetName().Name);
                       m.MigrationsHistoryTable($"__VtuDataOrderedSagaDbContext");
                   });
               });
           });



            busconfigurator.AddConfigureEndpointsCallback((context, name, cfg) =>
            {
                cfg.UseDelayedRedelivery(r => r.Intervals(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(15), TimeSpan.FromMinutes(30)));

                //cfg.UseMessageRetry(r => r.Interval(5, 1000));

                cfg.UseInMemoryOutbox(context);

                cfg.UseKillSwitch(options => options
                    .SetActivationThreshold(10)
                    .SetTripThreshold(0.15)
                    .SetRestartTimeout(m: 1)
                );

                cfg.UseCircuitBreaker(cb =>
                {
                    cb.TrackingPeriod = TimeSpan.FromMinutes(1);
                    cb.TripThreshold = 15;
                    cb.ActiveThreshold = 10;
                    cb.ResetInterval = TimeSpan.FromMinutes(5);
                });

                const int ConcurrencyLimit = 20;
                cfg.PrefetchCount = ConcurrencyLimit;
            });

            busconfigurator.AddDelayedMessageScheduler();


            busconfigurator.UsingRabbitMq((context, cfg) =>
            {
                MessageBrokerSettings _mbSettings = context.GetRequiredService<MessageBrokerSettings>();

                cfg.Host(new Uri(_mbSettings.Host), hst =>
                {
                    hst.Username(_mbSettings.Username);
                    hst.Password(_mbSettings.Password);
                });

                cfg.UseDelayedMessageScheduler();

                cfg.ConfigureEndpoints(context);
            });

            busconfigurator.ConfigureHealthCheckOptions(options =>
            {
                options.Name = "masstransit";
                options.MinimalFailureStatus = HealthStatus.Unhealthy;

                options.Tags.Add("health");
                options.Tags.Add("ready");
                options.Tags.Add("masstransit");
            });
        });
    }
}
