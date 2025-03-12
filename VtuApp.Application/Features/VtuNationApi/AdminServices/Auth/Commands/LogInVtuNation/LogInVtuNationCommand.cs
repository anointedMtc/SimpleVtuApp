using MediatR;
using VtuApp.Shared.DTO.VtuNationApi.AdminServices;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Auth.Commands.LogInVtuNation;

public sealed class LogInVtuNationCommand : IRequest<LogInVtuNationResponse>
{
    public LoginRequestVtuNation LoginRequestVtuNation { get; set; }
}
