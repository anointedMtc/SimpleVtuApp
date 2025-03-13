using MediatR;
using Microsoft.Extensions.Logging;
using VtuApp.Application.Interfaces.ExternalServices.VtuNationApi;

namespace VtuApp.Application.Features.VtuNationApi.UserServices.Queries.DataPrices.GetGloDataPrices;

public class GetGloDataPricesQueryHandler : IRequestHandler<GetGloDataPricesQuery, GetGloDataPricesResponse>
{
    private readonly IGetServicesFromVtuNation _getServicesFromVtuNation;
    private readonly ILogger<GetGloDataPricesQueryHandler> _logger;

    public GetGloDataPricesQueryHandler(IGetServicesFromVtuNation getServicesFromVtuNation, 
        ILogger<GetGloDataPricesQueryHandler> logger)
    {
        _getServicesFromVtuNation = getServicesFromVtuNation;
        _logger = logger;
    }

    public async Task<GetGloDataPricesResponse> Handle(GetGloDataPricesQuery request, CancellationToken cancellationToken)
    {
        var getGloDataPricesResponse = new GetGloDataPricesResponse();

        var response = await _getServicesFromVtuNation.GetGloDataPricesAsync();

        if (response.IsSuccessful)
        {
            getGloDataPricesResponse.AvailableDataPricesVtuNation = response.Content;
            getGloDataPricesResponse.Success = true;
            getGloDataPricesResponse.Message = $"Successfully retrieved available 9Mobile Data prices";
        }
        else
        {
            _logger.LogError("Unable to retrieve {NameOfRequest} from External Api {Name} at {time} with error message {Error.Message}",
                nameof(GetGloDataPricesQuery),
                "VtuNationApi",
                DateTimeOffset.UtcNow,
                response.Error.Message
            );

            // if response is null, it returns an empty list or collection
            getGloDataPricesResponse.Success = false;
            getGloDataPricesResponse.Message = $"--- {response.StatusCode} --- {response.Error.Message} --- {response.Error.InnerException}";
        }

        return getGloDataPricesResponse;
    }
}
