using SharedKernel.Application.DTO;
using VtuApp.Shared.DTO.VtuNationApi.AdminServices.Funding;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Funding.Queries.GetFundNotificationsVtuNation;

public sealed class GetFundNotificationsVtuNationResponse : ApiBaseResponse
{
    public GetFundNotificationsResponseVtuNation? GetFundNotificationsResponseVtuNation { get; set; }
}
