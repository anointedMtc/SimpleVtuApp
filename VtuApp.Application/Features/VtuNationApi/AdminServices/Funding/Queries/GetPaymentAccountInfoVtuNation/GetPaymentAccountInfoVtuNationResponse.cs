using ApplicationSharedKernel.DTO;
using VtuApp.Shared.DTO.VtuNationApi.AdminServices.Funding;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Funding.Queries.GetPaymentAccountInfoVtuNation;

public sealed class GetPaymentAccountInfoVtuNationResponse : ApiBaseResponse
{
    public PaymentAccountInfoResponseVtuNation PaymentAccountInfoResponseVtuNation { get; set; }
}
