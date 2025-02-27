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
        //builder.Configuration.AddJsonFile("notificationModuleSettings.json", false, true);

        //ConfigureControllers(builder);
        //ConfigureModuleFileProvidersAndSettingsFiles(builder);
        //ConfigureDatabase(builder);

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

    //private static void ConfigureModuleFileProvidersAndSettingsFiles(WebApplicationBuilder builder)
    //{
    //    var assemblyPath = typeof(EmailController).Assembly.Location;
    //    var directory = Path.GetDirectoryName(assemblyPath);
    //    var fileProvider = new PhysicalFileProvider(directory!);
    //    builder.Services.AddSingleton<IFileProvider>(fileProvider);
    //    builder.Configuration.AddJsonFile(fileProvider, "notificationModuleSettings.json", false, true);
    //}

    //private static void ConfigureDatabase(WebApplicationBuilder builder)
    //{
    //    //var connectionString = builder.Configuration.GetSection("NotificationModuleDb").Value;
    //    //var connectionString = builder.Configuration.GetConnectionString("NotificationModuleDb");

    //    //builder.Services.AddDbContext<EmailDbContext>(options =>
    //    //        options.UseSqlServer(connectionString, o => o.EnableRetryOnFailure()));

    //}

    //private static void ConfigureControllers(WebApplicationBuilder builder)
    //{
    //    builder.Services.AddControllers()
    //                    .AddApplicationPart(typeof(EmailController).Assembly);

    //    //var assembly = typeof(EmailController).Assembly;
    //    //builder.Services.AddControllersWithViews()
    //    //                .AddApplicationPart(assembly);

    //}

}
