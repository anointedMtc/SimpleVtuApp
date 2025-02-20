using MediatR;
using VtuApp.Shared.DTO.VtuNationApi.AdminServices.Account;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Account.Commands.SetUpdateTransactionPassVtuNation;

public sealed class SetUpdateTransactionPassVtuNationCommand : IRequest<SetUpdateTransactionPassVtuNationResponse>
{
    public SetUpdateTransactionPassRequestVtuNation SetUpdateTransactionPassRequestVtuNation { get; set; }
}
