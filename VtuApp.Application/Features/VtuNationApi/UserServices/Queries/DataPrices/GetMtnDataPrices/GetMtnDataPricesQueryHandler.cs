using MediatR;
using Microsoft.Extensions.Logging;
using VtuApp.Application.Interfaces.ExternalServices.VtuNationApi;

namespace VtuApp.Application.Features.VtuNationApi.UserServices.Queries.DataPrices.GetMtnDataPrices;

public class GetMtnDataPricesQueryHandler : IRequestHandler<GetMtnDataPricesQuery, GetMtnDataPricesResponse>
{
    private readonly IGetServicesFromVtuNation _getServicesFromVtuNation;
    private readonly ILogger<GetMtnDataPricesQueryHandler> _logger;

    public GetMtnDataPricesQueryHandler(IGetServicesFromVtuNation getServicesFromVtuNation, 
        ILogger<GetMtnDataPricesQueryHandler> logger)
    {
        _getServicesFromVtuNation = getServicesFromVtuNation;
        _logger = logger;
    }

    public async Task<GetMtnDataPricesResponse> Handle(GetMtnDataPricesQuery request, CancellationToken cancellationToken)
    {
        var getMtnDataPricesResponse = new GetMtnDataPricesResponse();

        var response = await _getServicesFromVtuNation.GetMtnDataPricesAsync();

        if (response.IsSuccessful)
        {
            getMtnDataPricesResponse.AvailableDataPricesVtuNation = response.Content;
            getMtnDataPricesResponse.Success = true;
            getMtnDataPricesResponse.Message = $"Successfully retrieved available 9Mobile Data prices";
        }
        else
        {
            _logger.LogError("Unable to retrieve {NameOfRequest} from External Api {Name} at {time}",
                nameof(GetMtnDataPricesQuery),
                "VtuNationApi",
                DateTimeOffset.UtcNow
            );

            // if response is null, it returns an empty list or collection
            getMtnDataPricesResponse.Success = false;
            getMtnDataPricesResponse.Message = $"Error processing your request. Please try again later";
        }

        return getMtnDataPricesResponse;
    }
}
