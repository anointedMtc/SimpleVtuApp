using MediatR;

namespace Wallet.Application.Features.Queries.GetWalletById;

public class GetWalletByIdQuery : IRequest<GetWalletByIdResponse>
{
    public Guid WalletId { get; set; }
}
