using ApplicationSharedKernel.DTO;
using VtuApp.Shared.DTO.VtuNationApi.UserServices;

namespace VtuApp.Application.Features.VtuNationApi.UserServices.Queries.GetAvailableDataNetworks;

public sealed class GetAvailableDataNetworksResponse : ApiBaseResponse
{
    public GetAvailableDataNetworksResponse()
    {
        AvailableDataNetworksResponseVtuNation = new();
    }
    public AvailableDataNetworksResponseVtuNation AvailableDataNetworksResponseVtuNation { get; set; }
}
