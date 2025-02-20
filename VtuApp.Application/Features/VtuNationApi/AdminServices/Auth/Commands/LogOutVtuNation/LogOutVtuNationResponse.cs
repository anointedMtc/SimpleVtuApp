using ApplicationSharedKernel.DTO;
using VtuApp.Shared.DTO.VtuNationApi.AdminServices.Auth;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Auth.Commands.LogOutVtuNation;

public sealed class LogOutVtuNationResponse : ApiBaseResponse
{
    public LogOutResponseVtuNation? LogOutResponseVtuNation { get; set; }
}
