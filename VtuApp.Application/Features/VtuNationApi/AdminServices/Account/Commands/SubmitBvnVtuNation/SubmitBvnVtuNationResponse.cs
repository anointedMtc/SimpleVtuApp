using ApplicationSharedKernel.DTO;
using VtuApp.Shared.DTO.VtuNationApi.AdminServices.Account;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Account.Commands.SubmitBvnVtuNation;

public sealed class SubmitBvnVtuNationResponse : ApiBaseResponse
{
    public SubmitBvnResponseVtuNation? SubmitBvnResponseVtuNation { get; set; }
}
