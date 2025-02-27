using MediatR;

namespace Wallet.Application.Features.Commands.DeleteOwner;

public sealed class DeleteOwnerCommand : IRequest<DeleteOwnerResponse>
{
    public string Email { get; set; }
}
