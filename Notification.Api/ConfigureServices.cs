using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Notification.Api.Controllers.V1;
using Notification.Application;
using Notification.Infrastructure;
using Notification.Infrastructure.Persistence;
using System;
using System.Reflection;

namespace Notification.Api;

public static class ConfigureServices
{
    public static void AddNotificationModule(this WebApplicationBuilder builder)
    {

        AddSettingsJsonFile(builder.Configuration);

        builder.Services.AddControllers()
                       .AddApplicationPart(typeof(Admin_EmailController).Assembly);

        builder.Services.AddNotificationApplicationLayer();
        builder.Services.AddNotificationInfrastructureLayer(builder.Configuration);
    }

    private static void AddSettingsJsonFile(this IConfigurationBuilder configurationBuilder)
    {

        var buildDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var filePath = buildDirectory + @"\notificationModuleSettings.json";
        configurationBuilder.AddJsonFile(filePath, false, true);

    }

}
