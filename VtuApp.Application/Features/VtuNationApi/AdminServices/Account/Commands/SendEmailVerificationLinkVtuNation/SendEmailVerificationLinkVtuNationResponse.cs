using ApplicationSharedKernel.DTO;
using VtuApp.Shared.DTO.VtuNationApi.AdminServices.Account;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Account.Commands.SendEmailVerificationLinkVtuNation;

public sealed class SendEmailVerificationLinkVtuNationResponse : ApiBaseResponse
{
    public SendEmailVerificationLinkResponseVtuNation? SendEmailVerificationLinkResponseVtuNation { get; set; }
}
