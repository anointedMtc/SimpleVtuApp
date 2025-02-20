using MediatR;

namespace Wallet.Application.Features.Commands.AddFunds;

public class AddFundsCommand : IRequest<AddFundsResponse>
{
    public Guid WalletId { get; set; }
    public decimal Amount { get; set; }
    public string ReasonWhy { get; set; }
}
