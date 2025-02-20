using MediatR;
using VtuApp.Shared.DTO.VtuNationApi.AdminServices.Account;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Account.Commands.ConfirmEmailVtuNation;

public sealed class ConfirmEmailVtuNationCommand : IRequest<ConfirmEmailVtuNationResponse>
{
    public ConfirmEmailRequestVtuNation ConfirmEmailRequestVtuNation { get; set; }
}
