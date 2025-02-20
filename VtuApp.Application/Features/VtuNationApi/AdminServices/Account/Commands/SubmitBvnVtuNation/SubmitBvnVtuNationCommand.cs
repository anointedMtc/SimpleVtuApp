using MediatR;
using VtuApp.Shared.DTO.VtuNationApi.AdminServices.Account;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Account.Commands.SubmitBvnVtuNation;

public sealed class SubmitBvnVtuNationCommand : IRequest<SubmitBvnVtuNationResponse>
{
    public SubmitBvnRequestVtuNation SubmitBvnRequestVtuNation { get; set; }
}
