using Refit;
using VtuApp.Shared.DTO.VtuNationApi.AdminServices;

namespace VtuApp.Application.Interfaces.ExternalServices.VtuNationApi;

public interface IGetTokenFromVtuNation
{
    // seperated this one from other admin services because we register it without an Auth Delegating Handler... but "Some" other admin services requires Auth-Token so we would registed the IGetAdminServicesFromVtuNation with Auth Header handler too...
    [Post("/api/auth/login")]       // the base address = https://api.vtunation.com
    Task<ApiResponse<LoginResponseVtuNation>> GetVtuNationApiTokenAsync([Body] LoginRequestVtuNation LoginRequestVtuNation);

}
