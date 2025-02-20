using MediatR;
using Microsoft.Extensions.Logging;
using VtuApp.Application.Interfaces.ExternalServices.VtuNationApi;

namespace VtuApp.Application.Features.VtuNationApi.UserServices.Queries.GetAvailableDataNetworks;

internal sealed class GetAvailableDataNetworksQueryHandler : IRequestHandler<GetAvailableDataNetworksQuery, GetAvailableDataNetworksResponse>
{
    private readonly IGetServicesFromVtuNation _getServicesFromVtuNation;
    private readonly ILogger<GetAvailableDataNetworksQueryHandler> _logger;

    public GetAvailableDataNetworksQueryHandler(IGetServicesFromVtuNation getServicesFromVtuNation, 
        ILogger<GetAvailableDataNetworksQueryHandler> logger)
    {
        _getServicesFromVtuNation = getServicesFromVtuNation;
        _logger = logger;
    }

    public async Task<GetAvailableDataNetworksResponse> Handle(GetAvailableDataNetworksQuery request, CancellationToken cancellationToken)
    {
        var getAvailableDataNetworksResponse = new GetAvailableDataNetworksResponse();

        var response = await _getServicesFromVtuNation.GetAvailableDataNetworksAsync();

        if (response.IsSuccessful)
        {
            getAvailableDataNetworksResponse.AvailableDataNetworksResponseVtuNation = response.Content;
            getAvailableDataNetworksResponse.Success = true;
            getAvailableDataNetworksResponse.Message = $"Successfully retrieved available 9Mobile Data prices";
        }
        else
        {
            _logger.LogError("Unable to retrieve {NameOfRequest} from External Api {Name} at {time}",
                nameof(GetAvailableDataNetworksQuery),
                "VtuNationApi",
                DateTimeOffset.UtcNow
            );

            // if response is null, it returns an empty list or collection
            getAvailableDataNetworksResponse.Success = false;
            getAvailableDataNetworksResponse.Message = $"Error processing your request. Please try again later";
        }

        return getAvailableDataNetworksResponse;
    }
}
