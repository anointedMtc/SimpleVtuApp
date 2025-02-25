using SharedKernel.Application.DTO;
using VtuApp.Shared.DTO;

namespace VtuApp.Application.Features.VtuNationApi.UserServices.Commands.BuyDataVtuNation;

public sealed class BuyDataVtuNationResponse : ApiBaseResponse
{
    public BuyDataVtuNationResponse()
    {
        VtuDataPurchaseResponseDto = new();
    }
    public VtuDataPurchaseResponseDto? VtuDataPurchaseResponseDto { get; set; }
}
