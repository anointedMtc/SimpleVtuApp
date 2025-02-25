using SharedKernel.Application.DTO;
using VtuApp.Shared.DTO.VtuNationApi.AdminServices.Account;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Account.Commands.UpdateProfileVtuNation;

public sealed class UpdateProfileVtuNationResponse : ApiBaseResponse
{
    public UpdateProfileResponseVtuNation? UpdateProfileResponseVtuNation { get; set; }
}
