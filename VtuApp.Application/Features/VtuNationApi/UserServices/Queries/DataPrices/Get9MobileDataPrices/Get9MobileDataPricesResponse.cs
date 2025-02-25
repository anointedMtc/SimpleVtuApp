using SharedKernel.Application.DTO;
using VtuApp.Shared.DTO.VtuNationApi.UserServices;

namespace VtuApp.Application.Features.VtuNationApi.UserServices.Queries.DataPrices.Get9MobileDataPrices;

public class Get9MobileDataPricesResponse : ApiBaseResponse
{
    public Get9MobileDataPricesResponse()
    {
        AvailableDataPricesVtuNation = new();
    }
    public AvailableDataPricesVtuNation AvailableDataPricesVtuNation { get; set; }
}
