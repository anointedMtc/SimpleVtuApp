using SharedKernel.Application.DTO;
using Wallet.Shared.DTO;

namespace Wallet.Application.Features.Queries.GetAllOwners;

public sealed class GetAllOwnersResponse : ApiBaseResponse
{
    public List<OwnerDto>? OwnerDtos { get; set; }
}
