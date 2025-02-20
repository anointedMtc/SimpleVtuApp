using MediatR;

namespace Wallet.Application.Features.Commands.DeductFunds;

public class DeductFundsCommand : IRequest<DeductFundsResponse>
{
    public Guid WalletId { get; set; }
    public decimal Amount { get; set; }
}
