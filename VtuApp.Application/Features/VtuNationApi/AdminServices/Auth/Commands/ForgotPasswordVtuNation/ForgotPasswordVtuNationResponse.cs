using ApplicationSharedKernel.DTO;
using VtuApp.Shared.DTO.VtuNationApi.AdminServices.Auth;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Auth.Commands.ForgotPasswordVtuNation;

public sealed class ForgotPasswordVtuNationResponse : ApiBaseResponse
{
    public ForgotPasswordResponseVtuNation? ForgotPasswordResponseVtuNation { get; set; }
}
