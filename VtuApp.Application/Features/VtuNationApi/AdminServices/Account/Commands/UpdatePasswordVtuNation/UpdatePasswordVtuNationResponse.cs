using SharedKernel.Application.DTO;
using VtuApp.Shared.DTO.VtuNationApi.AdminServices.Account;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Account.Commands.UpdatePasswordVtuNation;

public sealed class UpdatePasswordVtuNationResponse : ApiBaseResponse
{
    public UpdatePasswordResponseVtuNation? UpdatePasswordResponseVtuNation { get; set; }
}
