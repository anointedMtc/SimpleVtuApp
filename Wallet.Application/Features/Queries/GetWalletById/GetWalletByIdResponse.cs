using ApplicationSharedKernel.DTO;
using Wallet.Shared.DTO;

namespace Wallet.Application.Features.Queries.GetWalletById;

public class GetWalletByIdResponse : ApiBaseResponse
{
    public WalletDto WalletDto { get; set; }
}
