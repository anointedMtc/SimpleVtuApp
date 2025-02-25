using SharedKernel.Application.DTO;
using VtuApp.Shared.DTO;

namespace VtuApp.Application.Features.VtuNationApi.UserServices.Commands.BuyDataVtuNation.Mtn.Buy2GB;

public sealed class Buy2GBVtuNationResponse : ApiBaseResponse
{
    public VtuDataPurchaseResponseDto? VtuDataPurchaseResponseDto { get; set; }

}
