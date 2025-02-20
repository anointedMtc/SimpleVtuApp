using ApplicationSharedKernel.DTO;
using VtuApp.Shared.DTO.VtuNationApi.AdminServices.Account;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Account.Commands.SetUpdateTransactionPassVtuNation;

public sealed class SetUpdateTransactionPassVtuNationResponse : ApiBaseResponse
{
    public SetUpdateTransactionPassResponseVtuNation? SetUpdateTransactionPassResponseVtuNation { get; set; }
}
