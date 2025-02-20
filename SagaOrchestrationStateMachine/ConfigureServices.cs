using DomainSharedKernel.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using SagaOrchestrationStateMachines.UserCreatedSagaOrchestrator.Helpers.Controllers.V1;
using SagaOrchestrationStateMachines.UserCreatedSagaOrchestrator.Helpers.Repository;
using SagaOrchestrationStateMachines.VtuAirtimeOrderedSagaOrchestrator.Helpers.Repository;
using SagaOrchestrationStateMachines.VtuDataOrderedSagaOrchestrator.Helpers.Repository;
using System.Reflection;

namespace SagaOrchestrationStateMachines;

public static class ConfigureServices
{
    public static void AddSagaStateMachinesModule(this WebApplicationBuilder builder)
    {
        ConfigureControllers(builder);
        ConfigureModuleFileProvidersAndSettingsFiles(builder);

        builder.Services.AddScoped(typeof(IRepository<>), typeof(UserCreatedSagaOrchestratorRepository<>));
        builder.Services.AddScoped(typeof(IRepository<>), typeof(VtuAirtimeSagaOrchestratorRepository<>));
        builder.Services.AddScoped(typeof(IRepository<>), typeof(VtuDataSagaOrchestratorRepository<>));

        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        builder.Services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
    }

    private static void ConfigureModuleFileProvidersAndSettingsFiles(WebApplicationBuilder builder)
    {
        var assemblyPath = typeof(UserCreatedSagaOrchestratorController).Assembly.Location;
        var directory = Path.GetDirectoryName(assemblyPath);
        var fileProvider = new PhysicalFileProvider(directory!);
        builder.Services.AddSingleton<IFileProvider>(fileProvider);
        builder.Configuration.AddJsonFile(fileProvider, "sagaStateMachinesModuleSettings.json", false, true);
    }

    private static void ConfigureControllers(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers()
                        .AddApplicationPart(typeof(UserCreatedSagaOrchestratorController).Assembly);
        
    }

}
