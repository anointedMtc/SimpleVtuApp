using FluentValidation;
using Humanizer.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SagaOrchestrationStateMachines.Infrastructure.Persistence;
using SagaOrchestrationStateMachines.Api.Controllers.V1;
using System.Reflection;
using MassTransit;
using SagaOrchestrationStateMachines.Infrastructure.Persistence.Repository;
using SagaOrchestrationStateMachines.Domain.Interfaces;

namespace SagaOrchestrationStateMachines;

public static class ConfigureServices
{
    public static void AddSagaStateMachinesModule(this WebApplicationBuilder builder)
    {
        builder.Configuration.AddSettingsJsonFile();

        ConfigureControllers(builder);

        builder.Services.AddScoped(typeof(ISagaStateMachineRepository<>), typeof(SagaStateMachineRepository<>));

        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        builder.Services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });


        var connectionString = builder.Configuration.GetConnectionString("SagaStateMachinesModuleDb")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        builder.Services.AddDbContext<SagaStateMachineDbContext>(options =>
               options.UseSqlServer(connectionString, o => o.EnableRetryOnFailure()));

    }

    
    private static void ConfigureControllers(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers()
                        .AddApplicationPart(typeof(Saga_UserCreated_OrchestratorController).Assembly);

    }

    private static void AddSettingsJsonFile(this IConfigurationBuilder configurationBuilder)
    {
        
        var buildDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var filePath = buildDirectory + @"\sagaStateMachinesModuleSettings.json";
        configurationBuilder.AddJsonFile(filePath, false, true);

    }

}
