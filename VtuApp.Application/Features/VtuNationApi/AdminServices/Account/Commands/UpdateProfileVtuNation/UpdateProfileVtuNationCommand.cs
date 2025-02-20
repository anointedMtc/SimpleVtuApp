using MediatR;
using VtuApp.Shared.DTO.VtuNationApi.AdminServices.Account;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Account.Commands.UpdateProfileVtuNation;

public sealed class UpdateProfileVtuNationCommand : IRequest<UpdateProfileVtuNationResponse>
{
    public UpdateProfileRequestVtuNation UpdateProfileRequestVtuNation { get; set; }
}
