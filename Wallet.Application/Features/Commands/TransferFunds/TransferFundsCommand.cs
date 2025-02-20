using MediatR;

namespace Wallet.Application.Features.Commands.TransferFunds;

public class TransferFundsCommand : IRequest<TransferFundsResponse>
{
    public Guid FromWalletId { get; set; }
    public Guid ToWalletId { get; set; }
    public decimal Amount { get; set; }
}
