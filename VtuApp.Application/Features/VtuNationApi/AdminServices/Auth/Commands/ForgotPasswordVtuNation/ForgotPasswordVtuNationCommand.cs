using MediatR;
using VtuApp.Shared.DTO.VtuNationApi.AdminServices.Auth;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Auth.Commands.ForgotPasswordVtuNation;

public sealed class ForgotPasswordVtuNationCommand : IRequest<ForgotPasswordVtuNationResponse>    
{
    public ForgotPasswordRequestVtuNation ForgotPasswordRequestVtuNation { get; set; }
}
