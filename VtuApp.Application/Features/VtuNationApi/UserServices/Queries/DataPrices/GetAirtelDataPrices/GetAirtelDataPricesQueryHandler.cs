using MediatR;
using Microsoft.Extensions.Logging;
using VtuApp.Application.Interfaces.ExternalServices.VtuNationApi;

namespace VtuApp.Application.Features.VtuNationApi.UserServices.Queries.DataPrices.GetAirtelDataPrices;

public class GetAirtelDataPricesQueryHandler : IRequestHandler<GetAirtelDataPricesQuery, GetAirtelDataPricesResponse>
{
    private readonly IGetServicesFromVtuNation _getServicesFromVtuNation;
    private readonly ILogger<GetAirtelDataPricesQueryHandler> _logger;

    public GetAirtelDataPricesQueryHandler(IGetServicesFromVtuNation getServicesFromVtuNation, 
        ILogger<GetAirtelDataPricesQueryHandler> logger)
    {
        _getServicesFromVtuNation = getServicesFromVtuNation;
        _logger = logger;
    }

    public async Task<GetAirtelDataPricesResponse> Handle(GetAirtelDataPricesQuery request, CancellationToken cancellationToken)
    {
        var getAirtelDataPricesResponse = new GetAirtelDataPricesResponse();

        var response = await _getServicesFromVtuNation.GetAirtelDataPricesAsync();

        if (response.IsSuccessful)
        {
            getAirtelDataPricesResponse.AvailableDataPricesVtuNation = response.Content;
            getAirtelDataPricesResponse.Success = true;
            getAirtelDataPricesResponse.Message = $"Successfully retrieved available 9Mobile Data prices";
        }
        else
        {
            _logger.LogError("Unable to retrieve {NameOfRequest} from External Api {Name} at {time} with error message {Error.Message}",
                nameof(GetAirtelDataPricesQuery),
                "VtuNationApi",
                DateTimeOffset.UtcNow,
                response.Error.Message
            );

            // if response is null, it returns an empty list or collection
            getAirtelDataPricesResponse.Success = false;
            getAirtelDataPricesResponse.Message = $"--- {response.StatusCode} --- {response.Error.Message} --- {response.Error.InnerException}";
        }

        return getAirtelDataPricesResponse;
    }
}
