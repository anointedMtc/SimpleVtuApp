using ApplicationSharedKernel.DTO;
using VtuApp.Shared.DTO.VtuNationApi.AdminServices.Account;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Account.Commands.GenerateOtpVtuNation;

public sealed class GenerateOtpVtuNationResponse : ApiBaseResponse
{
    public GenerateOtpResponseVtuNation? GenerateOtpResponseVtuNation { get; set; }
}
