using SharedKernel.Application.DTO;
using Wallet.Shared.DTO;

namespace Wallet.Application.Features.Queries.GetAllWallets;

public sealed class GetAllWalletsResponse : ApiBaseResponse
{
    public List<WalletShortResponseDto>? WalletShortResponseDtos { get; set; }
}
