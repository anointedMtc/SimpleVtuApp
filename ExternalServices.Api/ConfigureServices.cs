using ExternalServices.Api.Controllers.TypiCodeService;
using ExternalServices.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;

namespace ExternalServices.Api;

public static class ConfigureServices // ConfigureExtensionsExternalServicesModule
                                      // ConfigureExtensionsIdentityModule
{
    public static void AddExternalServicesModule(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers()
                       .AddApplicationPart(typeof(TypiCodeController).Assembly);

        builder.Services.AddExternalServicesApplicationLayer();


        //builder.Configuration.AddJsonFile("externalServicesModuleSettings.json", false, true);

        //ConfigureModuleFileProvidersAndSettingsFiles(builder);
        //ConfigureControllers(builder);

    }
    //private static void ConfigureModuleFileProvidersAndSettingsFiles(WebApplicationBuilder builder)
    //{
    //    var assemblyPath = typeof(TypiCodeController).Assembly.Location;
    //    var directory = Path.GetDirectoryName(assemblyPath);
    //    var fileProvider = new PhysicalFileProvider(directory!);
    //    builder.Services.AddSingleton<IFileProvider>(fileProvider);
    //    builder.Configuration.AddJsonFile(fileProvider, "externalServicesModuleSettings.json", false, true);
    //}
    
    //private static void ConfigureControllers(WebApplicationBuilder builder)
    //{
    //    builder.Services.AddControllers()
    //                    .AddApplicationPart(typeof(TypiCodeController).Assembly);
    //}

    
}
