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

        // YOU DON'T NEED TO REGISTER THE SAGA-DB-CONTEXT AGAIN... IT HAS ALREADY BEEN REGISTERED FOR YOU - 
        // ADDING THIS REGISTERATION BELOW WON'T WORK BECAUSE IT TAKES A DEPENDENCY ON THE SAGA-DB-CONTEXT AND THE FOUNDER MENTIONED ON STACK OVERFLOW THAT THE REGISTERATION OF SAGADBCONTEXT IS DYNAMIC... IT COULD BE SINGLETON WHEN IT NEEDS IT LIKE THAT OR SCOPED OR TRANSIENT...
        // SO INSTEAD... WE USE THE DBCONTEXT DIRECTLY IN OUR HANDLERS SO IT RESOLVES THE SERVICE-LIFE-TIME DYNAMICALLY AT RUNTIME
        //builder.Services.AddScoped(typeof(IMySagaRepository<>), typeof(UserCreatedSagaOrchestratorRepository<>));
        //builder.Services.AddScoped(typeof(IRepository<>), typeof(VtuAirtimeSagaOrchestratorRepository<>));
        //builder.Services.AddScoped(typeof(IRepository<>), typeof(VtuDataSagaOrchestratorRepository<>));
        //builder.Services.AddScoped(typeof(IRepository<>), typeof(MySagaRepository<>));


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

    //private static void ConfigureModuleFileProvidersAndSettingsFiles(WebApplicationBuilder builder)
    //{
    //    var assemblyPath = typeof(UserCreatedSagaOrchestratorController).Assembly.Location;
    //    var directory = Path.GetDirectoryName(assemblyPath);
    //    var fileProvider = new PhysicalFileProvider(directory!);
    //    builder.Services.AddSingleton<IFileProvider>(fileProvider);
    //    builder.Configuration.AddJsonFile(fileProvider, "sagaStateMachinesModuleSettings.json", false, true);
    //}

    private static void ConfigureControllers(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers()
                        .AddApplicationPart(typeof(Saga_UserCreated_OrchestratorController).Assembly);

    }


    private static void AddSettingsJsonFile(this IConfigurationBuilder configurationBuilder)
    {
        //var assemblyPath = typeof(UserCreatedSagaOrchestratorController).Assembly.Location;
        //var directory = Path.GetDirectoryName(assemblyPath);
        //configurationBuilder.AddJsonFile($"/{directory}/sagaStateMachinesModuleSettings.json", false, true);


        //configurationBuilder.SetBasePath(Directory.GetCurrentDirectory())
        //                    .AddJsonFile("sagaStateMachinesModuleSettings.json", false, true);



        // interesting magic is that select the .json file, click properties and change the "Copy to Output Directory Property" to either copy always or copy if newer....
        // the summary is that Asp.Net will always look for the file in the executing assembly instead of the class library dll...
        // https://stackoverflow.com/questions/25419694/get-relative-file-path-in-a-class-library-project-that-is-being-refrenced-by-a

        // https://stackoverflow.com/questions/49650886/not-getting-path-to-class-library-project-from-within-class-library-project
        // A class library is compiled into the application that refrences it. After
        // build/publish, the DLL for the library resides in the same directory as all
        // the other DLLs for your application, meaning any paths will always be relative
        // to your web app directory, not your class library directory. ... if there's 
        // some file or files in your class library project directory that your class 
        // library needs to reference, you need to add them to your project and set them 
        // to copy on build in the properties pane for each file in Visual Studio. This
        // will result in the file(s) coming along for the ride and ending up in your
        // web app's build/publish directory as well. Your paths will still be relative
        // to the web app, not the class library... Alternatively, you can have a build
        // task that does the copy instead, but that's a little more complicated to set up

        // whether you do the above or you don't, it would always look for the file at
        // C:\\Users\\HP\\Desktop\\VisualStudioDocs\\SpecificationPattern\\DDDanointedMTC\\VtuHost.WebApi\\sagaStateMachinesModuleSettings.json
        // but what we want instead is...
        // C:\Users\HP\Desktop\VisualStudioDocs\SpecificationPattern\DDDanointedMTC\SagaOrchestrationStateMachine\sagaStateMachinesModuleSettings.json

        /*
         if you click the class library that contains the file, you would now see this added 

          <ItemGroup>
            <None Update="sagaStateMachinesModuleSettings.json">
              <CopyToOutputDirectory>Always</CopyToOutputDirectory>
            </None>
          </ItemGroup>

         */

        var buildDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var filePath = buildDirectory + @"\sagaStateMachinesModuleSettings.json";
        configurationBuilder.AddJsonFile(filePath, false, true);



    }

}
