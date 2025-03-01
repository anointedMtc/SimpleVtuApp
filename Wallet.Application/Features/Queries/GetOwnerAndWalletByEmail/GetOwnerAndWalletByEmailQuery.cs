using MediatR;

namespace Wallet.Application.Features.Queries.GetOwnerAndWalletByEmail;

public sealed class GetOwnerAndWalletByEmailQuery : IRequest<GetOwnerAndWalletByEmailResponse>
{
    public string Email { get; set; }
}
