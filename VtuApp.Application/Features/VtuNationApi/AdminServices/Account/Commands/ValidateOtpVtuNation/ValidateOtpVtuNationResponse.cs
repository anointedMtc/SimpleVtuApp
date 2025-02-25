using SharedKernel.Application.DTO;
using VtuApp.Shared.DTO.VtuNationApi.AdminServices.Account;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Account.Commands.ValidateOtpVtuNation;

public sealed class ValidateOtpVtuNationResponse : ApiBaseResponse
{
    public ValidateOtpResponseVtuNation? ValidateOtpResponseVtuNation { get; set; }
}
