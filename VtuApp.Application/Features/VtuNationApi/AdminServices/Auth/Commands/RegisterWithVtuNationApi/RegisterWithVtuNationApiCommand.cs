using MediatR;
using VtuApp.Shared.DTO.VtuNationApi.AdminServices.Auth;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Auth.Commands.RegisterWithVtuNationApi;

public sealed class RegisterWithVtuNationApiCommand : IRequest<RegisterWithVtuNationApiResponse>
{
    public RegisterRequestVtuNation RegisterRequestVtuNation { get; set; }
}
