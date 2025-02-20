using ApplicationSharedKernel.DTO;
using VtuApp.Shared.DTO.VtuNationApi.UserServices;

namespace VtuApp.Application.Features.VtuNationApi.UserServices.Queries.GetAvailableAirtimeNetworks;

public sealed class GetAvailableAirtimeNetworksResponse : ApiBaseResponse
{
    public GetAvailableAirtimeNetworksResponse()
    {
        AvailableAirtimeNetworksResponseVtuNation = new();
    }
    public AvailableAirtimeNetworksResponseVtuNation AvailableAirtimeNetworksResponseVtuNation { get; set; }
}
