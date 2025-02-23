using DomainSharedKernel.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SagaOrchestrationStateMachines.UserCreatedSagaOrchestrator.Helpers.Controllers.V1;
using System.Reflection;

namespace SagaOrchestrationStateMachines;

public static class ConfigureServices
{
    public static void AddSagaStateMachinesModule(this WebApplicationBuilder builder)
    {
        AddSettingsJsonFile(builder.Configuration);

        ConfigureControllers(builder);
        //ConfigureModuleFileProvidersAndSettingsFiles(builder);

        //builder.Services.AddScoped(typeof(IRepository<>), typeof(UserCreatedSagaOrchestratorRepository<>));
        //builder.Services.AddScoped(typeof(IRepository<>), typeof(VtuAirtimeSagaOrchestratorRepository<>));
        //builder.Services.AddScoped(typeof(IRepository<>), typeof(VtuDataSagaOrchestratorRepository<>));

        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        builder.Services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
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
                        .AddApplicationPart(typeof(UserCreatedSagaOrchestratorController).Assembly);
        
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

        var buildDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var filePath = buildDirectory + @"\sagaStateMachinesModuleSettings.json";
        configurationBuilder.AddJsonFile(filePath, false, true);



    }

}
