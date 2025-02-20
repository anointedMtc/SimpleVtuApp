using ApplicationSharedKernel;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Api.Controllers;
using SharedKernel.Infrastructure;

namespace SharedKernel.Api;

public static class ConfigureServices
{
    public static void AddSharedKernelModule(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers()
                        .AddApplicationPart(typeof(ApiBaseController).Assembly);

        builder.Services.AddSharedKernelApplicationServices();
        builder.Services.AddSharedKernelInfrastructureServices();
    }
}
