using MediatR;
using VtuApp.Shared.DTO.VtuNationApi.AdminServices.Account;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Account.Commands.UpdatePasswordVtuNation;

public sealed class UpdatePasswordVtuNationCommand : IRequest<UpdatePasswordVtuNationResponse>
{
    public UpdatePasswordRequestVtuNation UpdatePasswordRequestVtuNation { get; set; }
}
