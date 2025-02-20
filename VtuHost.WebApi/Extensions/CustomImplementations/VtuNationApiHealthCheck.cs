using Microsoft.Extensions.Diagnostics.HealthChecks;
using VtuApp.Application.Interfaces.ExternalServices.VtuNationApi;

namespace VtuHost.WebApi.Extensions.CustomImplementations;

public class VtuNationApiHealthCheck : IHealthCheck
{
    private readonly IGetServicesFromVtuNation _getServicesFromVtuNation;

    public VtuNationApiHealthCheck(IGetServicesFromVtuNation getServicesFromVtuNation)
    {
        _getServicesFromVtuNation = getServicesFromVtuNation;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var response = await _getServicesFromVtuNation.GetAvailableAirtimeNetworksAsync();

        if (response.IsSuccessStatusCode)
        {
            return await Task.FromResult(new HealthCheckResult(
                status: HealthStatus.Healthy,
                description: "The API is up and running.")
            );
        }
        return await Task.FromResult(new HealthCheckResult(
            status: HealthStatus.Unhealthy,
            description: "The API is down.")
        );
    }
}
