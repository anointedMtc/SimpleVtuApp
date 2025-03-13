using SharedKernel.Application.DTO;
using VtuApp.Shared.DTO.VtuNationApi.AdminServices;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Auth.Commands.LogInVtuNation;

public sealed class LogInVtuNationResponse : ApiBaseResponse
{
    public LoginResponseVtuNation LoginResponseVtuNation { get; set; }
}
