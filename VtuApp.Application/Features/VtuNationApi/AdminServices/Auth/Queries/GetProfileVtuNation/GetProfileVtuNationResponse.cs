using SharedKernel.Application.DTO;
using VtuApp.Shared.DTO.VtuNationApi.AdminServices.Auth;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Auth.Queries.GetProfileVtuNation;

public sealed class GetProfileVtuNationResponse : ApiBaseResponse
{
    public GetProfileResponseVtuNation? GetProfileResponseVtuNation { get; set; }
}
