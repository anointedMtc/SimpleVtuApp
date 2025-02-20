using MediatR;
using Microsoft.Extensions.Logging;
using VtuApp.Application.Interfaces.ExternalServices.VtuNationApi;

namespace VtuApp.Application.Features.VtuNationApi.UserServices.Queries.DataPrices.Get9MobileDataPrices;

public class Get9MobileDataPricesQueryHandler : IRequestHandler<Get9MobileDataPricesQuery, Get9MobileDataPricesResponse>
{
    private readonly IGetServicesFromVtuNation _getServicesFromVtuNation;
    private readonly ILogger<Get9MobileDataPricesQueryHandler> _logger;
    

    public Get9MobileDataPricesQueryHandler(IGetServicesFromVtuNation getServicesFromVtuNation, 
        ILogger<Get9MobileDataPricesQueryHandler> logger)
    {
        _getServicesFromVtuNation = getServicesFromVtuNation;
        _logger = logger;
    }

    public async Task<Get9MobileDataPricesResponse> Handle(Get9MobileDataPricesQuery request, CancellationToken cancellationToken)
    {
        // NO NEED FOR AUTH... ANYONE SHOULD BE ABLE TO SEE OUR LIST OF AVAILABLE NEWTORKS AND PROBABLY THEIR PRICES... BUT NOT ANYONE CAN BUY

        // this would work... but because I "newed" it up in the constructor, no need to new it up again here...
        //var get9MobileDataPricesResponse = new Get9MobileDataPricesResponse
        //{
        //    AvailableDataPricesVtuNation = new()
        //};

        var get9MobileDataPricesResponse = new Get9MobileDataPricesResponse();

        var response = await _getServicesFromVtuNation.Get9MobileDataPricesAsync();

        if (response.IsSuccessful)
        {
            get9MobileDataPricesResponse.AvailableDataPricesVtuNation = response.Content;
            get9MobileDataPricesResponse.Success = true;
            get9MobileDataPricesResponse.Message = $"Successfully retrieved available 9Mobile Data prices";
        }
        else
        {
            _logger.LogError("Unable to retrieve {NameOfRequest} from External Api {Name} at {time}",
                nameof(Get9MobileDataPricesQuery),
                "VtuNationApi",
                DateTimeOffset.UtcNow
            );

            // if response is null, it returns an empty list or collection
            get9MobileDataPricesResponse.Success = false;
            get9MobileDataPricesResponse.Message = $"Error processing your request. Please try again later";
        }
        
        return get9MobileDataPricesResponse;
    }
}
