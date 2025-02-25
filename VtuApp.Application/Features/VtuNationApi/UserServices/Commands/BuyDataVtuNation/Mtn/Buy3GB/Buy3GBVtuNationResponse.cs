using SharedKernel.Application.DTO;
using VtuApp.Shared.DTO;

namespace VtuApp.Application.Features.VtuNationApi.UserServices.Commands.BuyDataVtuNation.Mtn.Buy3GB;

public sealed class Buy3GBVtuNationResponse : ApiBaseResponse
{
    public VtuDataPurchaseResponseDto? VtuDataPurchaseResponseDto { get; set; }

}
