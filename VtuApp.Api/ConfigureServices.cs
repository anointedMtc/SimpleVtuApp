﻿using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using VtuApp.Api.Controllers.V1;
using VtuApp.Application;
using VtuApp.Infrastructure;
using VtuApp.Infrastructure.Persistence;

namespace VtuApp.Api;

public static class ConfigureServices
{
    public static void AddVtuAppModule(this WebApplicationBuilder builder)
    {
        ConfigureControllers(builder);
        ConfigureModuleFileProvidersAndSettingsFiles(builder);
        ConfigureDatabase(builder);

        builder.Services.AddVtuAppApplicationLayer();
        builder.Services.AddVtuAppInfrastructureLayer();
    }

    private static void ConfigureModuleFileProvidersAndSettingsFiles(WebApplicationBuilder builder)
    {
        var assemblyPath = typeof(UserServicesVtuNationController).Assembly.Location;
        var directory = Path.GetDirectoryName(assemblyPath);
        var fileProvider = new PhysicalFileProvider(directory!);
        builder.Services.AddSingleton<IFileProvider>(fileProvider);
        builder.Configuration.AddJsonFile(fileProvider, "vtuAppModuleSettings.json", false, true);
    }

    private static void ConfigureDatabase(WebApplicationBuilder builder)
    {
        //var connectionString = builder.Configuration.GetSection("VtuAppModuleDb").Value;
        var connectionString = builder.Configuration.GetConnectionString("VtuAppModuleDb");

        builder.Services.AddDbContext<VtuDbContext>(options =>
                options.UseSqlServer(connectionString, o => o.EnableRetryOnFailure()));

    }

    private static void ConfigureControllers(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers()
                        .AddApplicationPart(typeof(UserServicesVtuNationController).Assembly);

        //var assembly = typeof(UserServicesVtuNationController).Assembly;
        //builder.Services.AddControllersWithViews()
        //                .AddApplicationPart(assembly);

    }

}
