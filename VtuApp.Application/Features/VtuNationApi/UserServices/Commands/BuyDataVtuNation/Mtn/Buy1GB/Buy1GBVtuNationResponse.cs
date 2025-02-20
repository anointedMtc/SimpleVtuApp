using ApplicationSharedKernel.DTO;
using VtuApp.Shared.DTO;

namespace VtuApp.Application.Features.VtuNationApi.UserServices.Commands.BuyDataVtuNation.Mtn.Buy1GB;

public sealed class Buy1GBVtuNationResponse : ApiBaseResponse
{
    public VtuDataPurchaseResponseDto? VtuDataPurchaseResponseDto { get; set; }

}
