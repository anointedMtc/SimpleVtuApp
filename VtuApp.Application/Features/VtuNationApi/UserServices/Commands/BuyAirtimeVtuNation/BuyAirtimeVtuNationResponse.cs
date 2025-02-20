using ApplicationSharedKernel.DTO;
using VtuApp.Shared.DTO;

namespace VtuApp.Application.Features.VtuNationApi.UserServices.Commands.BuyAirtimeVtuNation;

public sealed class BuyAirtimeVtuNationResponse : ApiBaseResponse
{
    public BuyAirtimeVtuNationResponse()
    {
        VtuAirtimePurchaseResponseDto = new();
    }
    public VtuAirtimePurchaseResponseDto? VtuAirtimePurchaseResponseDto { get; set; }

}
