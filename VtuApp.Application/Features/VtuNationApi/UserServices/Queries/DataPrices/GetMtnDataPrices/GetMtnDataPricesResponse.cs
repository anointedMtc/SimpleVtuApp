using SharedKernel.Application.DTO;
using VtuApp.Shared.DTO.VtuNationApi.UserServices;

namespace VtuApp.Application.Features.VtuNationApi.UserServices.Queries.DataPrices.GetMtnDataPrices;

public class GetMtnDataPricesResponse : ApiBaseResponse
{
    public GetMtnDataPricesResponse()
    {
        AvailableDataPricesVtuNation = new();
    }

    public AvailableDataPricesVtuNation AvailableDataPricesVtuNation { get; set; }

}
