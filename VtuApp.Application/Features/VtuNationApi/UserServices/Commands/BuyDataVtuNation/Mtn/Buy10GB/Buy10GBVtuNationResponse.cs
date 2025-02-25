using SharedKernel.Application.DTO;
using VtuApp.Shared.DTO;

namespace VtuApp.Application.Features.VtuNationApi.UserServices.Commands.BuyDataVtuNation.Mtn.Buy10GB;

public sealed class Buy10GBVtuNationResponse : ApiBaseResponse
{
    public VtuDataPurchaseResponseDto? VtuDataPurchaseResponseDto { get; set; }

}
