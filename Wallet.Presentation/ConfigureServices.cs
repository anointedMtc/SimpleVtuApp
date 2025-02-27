using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Wallet.Infrastructure;
using Wallet.Application;
using Microsoft.Extensions.Configuration;
using Wallet.Api.Controllers.V1;
using System.Reflection;

namespace Wallet.Api;

public static class ConfigureServices
{
    public static void AddWalletsModule(this WebApplicationBuilder builder)
    {
        AddSettingsJsonFile(builder.Configuration);

        builder.Services.AddControllers()
                        .AddApplicationPart(typeof(WalletController).Assembly);

        builder.Services.AddWalletApplicationLayer();
        builder.Services.AddWalletInfrastructureLayer(builder.Configuration);
    }

    private static void AddSettingsJsonFile(this IConfigurationBuilder configurationBuilder)
    {

        var buildDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var filePath = buildDirectory + @"\walletModuleSettings.json";
        configurationBuilder.AddJsonFile(filePath, false, true);

    }
    
}
