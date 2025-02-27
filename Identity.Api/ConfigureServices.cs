using Identity.Api.Controllers.V1;
using Identity.Application;
using Identity.Infrastructure;
using Identity.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.Reflection;

namespace Identity.Api;

public static class ConfigureServices
{
    public static void AddIdentityModule(this WebApplicationBuilder builder)
    {

        AddSettingsJsonFile(builder.Configuration);

        builder.Services.AddControllers()
                       .AddApplicationPart(typeof(AccountController).Assembly);

        builder.Services.AddIdentityApplicationLayer();
        builder.Services.AddIdentityInfrastructureLayer(builder.Configuration);
    }

    private static void AddSettingsJsonFile(this IConfigurationBuilder configurationBuilder)
    {

        var buildDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var filePath = buildDirectory + @"\identityModuleSettings.json";
        configurationBuilder.AddJsonFile(filePath, false, true);

    }

}
