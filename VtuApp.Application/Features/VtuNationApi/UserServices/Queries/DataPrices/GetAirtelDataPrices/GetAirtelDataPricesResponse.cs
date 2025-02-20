using ApplicationSharedKernel.DTO;
using VtuApp.Shared.DTO.VtuNationApi.UserServices;

namespace VtuApp.Application.Features.VtuNationApi.UserServices.Queries.DataPrices.GetAirtelDataPrices;

public class GetAirtelDataPricesResponse : ApiBaseResponse
{
    public GetAirtelDataPricesResponse()
    {
        AvailableDataPricesVtuNation = new();
    }
    public AvailableDataPricesVtuNation AvailableDataPricesVtuNation { get; set; }
}
