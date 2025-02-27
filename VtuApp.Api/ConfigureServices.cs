using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.Reflection;
using VtuApp.Api.Controllers.V1;
using VtuApp.Application;
using VtuApp.Infrastructure;
using VtuApp.Infrastructure.Persistence;

namespace VtuApp.Api;

public static class ConfigureServices
{
    public static void AddVtuAppModule(this WebApplicationBuilder builder)
    {
        AddSettingsJsonFile(builder.Configuration);

        builder.Services.AddControllers()
                       .AddApplicationPart(typeof(VtuNation_UserServicesController).Assembly);

        builder.Services.AddVtuAppApplicationLayer();
        builder.Services.AddVtuAppInfrastructureLayer(builder.Configuration);

    }

    private static void AddSettingsJsonFile(this IConfigurationBuilder configurationBuilder)
    {

        var buildDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var filePath = buildDirectory + @"\vtuAppModuleSettings.json";
        configurationBuilder.AddJsonFile(filePath, false, true);

    }


}
