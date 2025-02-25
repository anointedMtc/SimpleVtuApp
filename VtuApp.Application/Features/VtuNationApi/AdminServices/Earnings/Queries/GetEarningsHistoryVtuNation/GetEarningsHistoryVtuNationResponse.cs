using SharedKernel.Application.DTO;
using VtuApp.Shared.DTO.VtuNationApi.AdminServices.Earnings;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Earnings.Queries.GetEarningsHistoryVtuNation;

public sealed class GetEarningsHistoryVtuNationResponse : ApiBaseResponse
{
    public GetEarningsHistoryResponseVtuNation? GetEarningsHistoryResponseVtuNation { get; set; }
}
