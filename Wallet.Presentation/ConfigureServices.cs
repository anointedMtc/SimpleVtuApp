using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Wallet.Infrastructure;
using Wallet.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Wallet.Api.Controllers.V1;
using Wallet.Infrastructure.Persistence;

namespace Wallet.Api;

public static class ConfigureServices
{
    public static void AddWalletsModule(this WebApplicationBuilder builder)
    {
        ConfigureControllers(builder);
        ConfigureModuleFileProvidersAndSettingsFiles(builder);
        ConfigureDatabase(builder);

        builder.Services.AddWalletApplicationLayer();
        builder.Services.AddWalletInfrastructureLayer();
    }

    private static void ConfigureModuleFileProvidersAndSettingsFiles(WebApplicationBuilder builder)
    {
        var assemblyPath = typeof(WalletController).Assembly.Location;
        var directory = Path.GetDirectoryName(assemblyPath);
        var fileProvider = new PhysicalFileProvider(directory!);
        builder.Services.AddSingleton<IFileProvider>(fileProvider);
        builder.Configuration.AddJsonFile(fileProvider, "walletModuleSettings.json", false, true);
    }

    private static void ConfigureDatabase(WebApplicationBuilder builder)
    {
        //var connectionString = builder.Configuration.GetSection("WalletModuleDb").Value;
        var connectionString = builder.Configuration.GetConnectionString("WalletModuleDb");

        builder.Services.AddDbContext<WalletDbContext>(options =>
                options.UseSqlServer(connectionString, o => o.EnableRetryOnFailure()));

    }

    private static void ConfigureControllers(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers()
                        .AddApplicationPart(typeof(WalletController).Assembly);

        //var assembly = typeof(WalletController).Assembly;
        //builder.Services.AddControllersWithViews()
        //                .AddApplicationPart(assembly);

    }

}
