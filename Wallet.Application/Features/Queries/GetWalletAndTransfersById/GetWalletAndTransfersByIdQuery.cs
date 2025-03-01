using MediatR;

namespace Wallet.Application.Features.Queries.GetWalletAndTransfersById;

public sealed class GetWalletAndTransfersByIdQuery : IRequest<GetWalletAndTransfersByIdResponse>
{
    public Guid WalletId { get; set; }
}
