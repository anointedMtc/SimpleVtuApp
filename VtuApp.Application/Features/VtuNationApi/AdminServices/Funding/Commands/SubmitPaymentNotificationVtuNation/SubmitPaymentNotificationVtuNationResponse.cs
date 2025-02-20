using ApplicationSharedKernel.DTO;
using VtuApp.Shared.DTO.VtuNationApi.AdminServices.Funding;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Funding.Commands.SubmitPaymentNotificationVtuNation;

public sealed class SubmitPaymentNotificationVtuNationResponse : ApiBaseResponse
{
    public SubmitPaymentNotificationResponseVtuNation SubmitPaymentNotificationResponseVtuNation { get; set; }
}
