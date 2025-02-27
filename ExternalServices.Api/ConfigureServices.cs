using ExternalServices.Api.Controllers.TypiCodeService;
using ExternalServices.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.Reflection;

namespace ExternalServices.Api;

public static class ConfigureServices 
{
    public static void AddExternalServicesModule(this WebApplicationBuilder builder)
    {
        AddSettingsJsonFile(builder.Configuration);

        builder.Services.AddControllers()
                       .AddApplicationPart(typeof(TypiCodeController).Assembly);

        builder.Services.AddExternalServicesApplicationLayer();

    }

    private static void AddSettingsJsonFile(this IConfigurationBuilder configurationBuilder)
    {

        var buildDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var filePath = buildDirectory + @"\externalServicesModuleSettings.json";
        configurationBuilder.AddJsonFile(filePath, false, true);

    }

}
