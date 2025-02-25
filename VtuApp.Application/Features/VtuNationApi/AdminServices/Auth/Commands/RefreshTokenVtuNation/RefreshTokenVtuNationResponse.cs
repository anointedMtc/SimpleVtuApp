using SharedKernel.Application.DTO;
using VtuApp.Shared.DTO.VtuNationApi.AdminServices.Auth;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Auth.Commands.RefreshTokenVtuNation;

public sealed class RefreshTokenVtuNationResponse : ApiBaseResponse
{
    public RefreshTokenResponseVtuNation? RefreshTokenResponseVtuNation { get; set; }
}
