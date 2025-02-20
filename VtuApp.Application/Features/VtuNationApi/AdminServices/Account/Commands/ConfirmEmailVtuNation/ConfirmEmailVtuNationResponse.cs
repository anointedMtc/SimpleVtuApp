using ApplicationSharedKernel.DTO;
using VtuApp.Shared.DTO.VtuNationApi.AdminServices.Account;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Account.Commands.ConfirmEmailVtuNation;

public sealed class ConfirmEmailVtuNationResponse : ApiBaseResponse
{
    public ConfirmEmailResponseVtuNation? ConfirmEmailResponseVtuNation { get; set; }
}
