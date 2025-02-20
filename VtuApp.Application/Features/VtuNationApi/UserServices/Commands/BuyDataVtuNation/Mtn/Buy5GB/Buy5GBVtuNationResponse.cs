using ApplicationSharedKernel.DTO;
using VtuApp.Shared.DTO;

namespace VtuApp.Application.Features.VtuNationApi.UserServices.Commands.BuyDataVtuNation.Mtn.Buy5GB;

public sealed class Buy5GBVtuNationResponse : ApiBaseResponse
{
    public VtuDataPurchaseResponseDto? VtuDataPurchaseResponseDto { get; set; }

}
