using SharedKernel.Application.DTO;
using Wallet.Shared.DTO;

namespace Wallet.Application.Features.Queries.GetWalletAndTransfersById;

public sealed class GetWalletAndTransfersByIdResponse : ApiBaseResponse
{
    public WalletDto? WalletDto { get; set; }
}
