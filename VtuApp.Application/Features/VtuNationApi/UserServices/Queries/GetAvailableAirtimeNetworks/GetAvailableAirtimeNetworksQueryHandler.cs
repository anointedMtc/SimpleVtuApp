using MediatR;
using Microsoft.Extensions.Logging;
using VtuApp.Application.Interfaces.ExternalServices.VtuNationApi;

namespace VtuApp.Application.Features.VtuNationApi.UserServices.Queries.GetAvailableAirtimeNetworks;

internal sealed class GetAvailableAirtimeNetworksQueryHandler : IRequestHandler<GetAvailableAirtimeNetworksQuery, GetAvailableAirtimeNetworksResponse>
{
    private readonly IGetServicesFromVtuNation _getServicesFromVtuNation;
    private readonly ILogger<GetAvailableAirtimeNetworksQueryHandler> _logger;

    public GetAvailableAirtimeNetworksQueryHandler(IGetServicesFromVtuNation getServicesFromVtuNation, 
        ILogger<GetAvailableAirtimeNetworksQueryHandler> logger)
    {
        _getServicesFromVtuNation = getServicesFromVtuNation;
        _logger = logger;
    }

    public async Task<GetAvailableAirtimeNetworksResponse> Handle(GetAvailableAirtimeNetworksQuery request, CancellationToken cancellationToken)
    {
        var getAvailableAirtimeNetworksResponse = new GetAvailableAirtimeNetworksResponse();

        var response = await _getServicesFromVtuNation.GetAvailableAirtimeNetworksAsync();

        if (response.IsSuccessful)
        {
            getAvailableAirtimeNetworksResponse.AvailableAirtimeNetworksResponseVtuNation = response.Content;
            getAvailableAirtimeNetworksResponse.Success = true;
            getAvailableAirtimeNetworksResponse.Message = $"Successfully retrieved available 9Mobile Data prices";
        }
        else
        {
            _logger.LogError("Unable to retrieve {NameOfRequest} from External Api {Name} at {time}",
                nameof(GetAvailableAirtimeNetworksQuery),
                "VtuNationApi",
                DateTimeOffset.UtcNow
            );

            // if response is null, it returns an empty list or collection
            getAvailableAirtimeNetworksResponse.Success = false;
            getAvailableAirtimeNetworksResponse.Message = $"Error processing your request. Please try again later";
        }

        return getAvailableAirtimeNetworksResponse;
    }
}
