using SharedKernel.Application.DTO;
using VtuApp.Shared.DTO.VtuNationApi.UserServices;

namespace VtuApp.Application.Features.VtuNationApi.UserServices.Queries.DataPrices.GetGloDataPrices;

public class GetGloDataPricesResponse : ApiBaseResponse
{
    public GetGloDataPricesResponse()
    {
        AvailableDataPricesVtuNation = new();
    }
    public AvailableDataPricesVtuNation AvailableDataPricesVtuNation { get; set; }

}
