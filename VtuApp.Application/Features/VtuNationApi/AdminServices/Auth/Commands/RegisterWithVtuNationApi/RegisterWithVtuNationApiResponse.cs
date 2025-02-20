using ApplicationSharedKernel.DTO;
using VtuApp.Shared.DTO.VtuNationApi.AdminServices.Auth;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Auth.Commands.RegisterWithVtuNationApi;

public sealed class RegisterWithVtuNationApiResponse : ApiBaseResponse
{
    public RegisterResponseVtuNation? RegisterResponseVtuNation { get; set; }
}
