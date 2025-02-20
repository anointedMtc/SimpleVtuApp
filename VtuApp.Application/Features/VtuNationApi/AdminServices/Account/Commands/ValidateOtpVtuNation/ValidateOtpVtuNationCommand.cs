using MediatR;
using VtuApp.Shared.DTO.VtuNationApi.AdminServices.Account;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Account.Commands.ValidateOtpVtuNation;

public sealed class ValidateOtpVtuNationCommand : IRequest<ValidateOtpVtuNationResponse>
{
    public ValidateOtpRequestVtuNation ValidateOtpRequestVtuNation { get; set; }
}
