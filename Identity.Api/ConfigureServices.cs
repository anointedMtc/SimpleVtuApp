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
        //builder.Configuration.AddJsonFile("IdentityModuleSettings.json", false, true);

        //ConfigureControllers(builder);
        //ConfigureModuleFileProvidersAndSettingsFiles(builder);
        //ConfigureDatabase(builder);

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

    //private static void ConfigureModuleFileProvidersAndSettingsFiles(WebApplicationBuilder builder)
    //{
    //    var assemblyPath = typeof(AccountController).Assembly.Location;
    //    var directory = Path.GetDirectoryName(assemblyPath);
    //    var fileProvider = new PhysicalFileProvider(directory!);
    //    builder.Services.AddSingleton<IFileProvider>(fileProvider);
    //    builder.Configuration.AddJsonFile(fileProvider, "IdentityModuleSettings.json", false, true);
    //}

    //private static void ConfigureDatabase(WebApplicationBuilder builder)
    //{
    //    //var connectionString = builder.Configuration.GetSection("IdentityModuleDb").Value;
    //    var connectionString = builder.Configuration.GetConnectionString("IdentityModuleDb");

    //    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    //            options.UseSqlServer(connectionString, o => o.EnableRetryOnFailure()));

    //}

    //private static void ConfigureControllers(WebApplicationBuilder builder)
    //{
    //    builder.Services.AddControllers()
    //                    .AddApplicationPart(typeof(AccountController).Assembly);

    //    //var assembly = typeof(AccountController).Assembly;
    //    //builder.Services.AddControllersWithViews()
    //    //                .AddApplicationPart(assembly);

    //}

}
