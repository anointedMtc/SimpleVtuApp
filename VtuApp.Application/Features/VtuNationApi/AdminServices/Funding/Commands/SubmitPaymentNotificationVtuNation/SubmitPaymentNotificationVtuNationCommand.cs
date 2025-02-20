using MediatR;
using VtuApp.Shared.DTO.VtuNationApi.AdminServices.Funding;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Funding.Commands.SubmitPaymentNotificationVtuNation;

public sealed class SubmitPaymentNotificationVtuNationCommand : IRequest<SubmitPaymentNotificationVtuNationResponse>
{
    public SubmitPaymentNotificationRequestVtuNation SubmitPaymentNotificationRequestVtuNation { get; set; }
}
