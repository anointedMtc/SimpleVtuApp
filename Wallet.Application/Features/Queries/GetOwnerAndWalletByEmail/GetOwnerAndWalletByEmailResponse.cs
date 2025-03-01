using SharedKernel.Application.DTO;
using Wallet.Shared.DTO;

namespace Wallet.Application.Features.Queries.GetOwnerAndWalletByEmail;

public sealed class GetOwnerAndWalletByEmailResponse : ApiBaseResponse
{
    public OwnerLongResponseDto? OwnerLongResponseDto { get; set; }
}
