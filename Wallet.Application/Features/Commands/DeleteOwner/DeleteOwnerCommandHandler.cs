using Identity.Shared.Constants;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Exceptions;
using SharedKernel.Application.Interfaces;
using SharedKernel.Domain.Interfaces;
using Wallet.Application.Features.Commands.DeductFunds;
using Wallet.Domain.Entities;
using Wallet.Domain.Entities.WalletAggregate;
using Wallet.Domain.Interfaces;
using Wallet.Domain.Specifications;

namespace Wallet.Application.Features.Commands.DeleteOwner;

internal sealed class DeleteOwnerCommandHandler : IRequestHandler<DeleteOwnerCommand, DeleteOwnerResponse>
{
    private readonly IWalletRepository<Owner> _walletRepository;
    private readonly ILogger<DeleteOwnerCommandHandler> _logger;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;

    public DeleteOwnerCommandHandler(IWalletRepository<Owner> walletRepository, 
        ILogger<DeleteOwnerCommandHandler> logger, IUserContext userContext,
        IResourceBaseAuthorizationService resourceBaseAuthorizationService)
    {
        _walletRepository = walletRepository;
        _logger = logger;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
    }

    public async Task<DeleteOwnerResponse> Handle(DeleteOwnerCommand request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.GodsEyeOnly))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand?.Email ?? "Anonymous User",
                nameof(DeleteOwnerCommand),
                request);

            throw new ForbiddenAccessException();
        }

        var deleteOwnerResponse = new DeleteOwnerResponse();

        var spec = new GetOwnerByEmailSpecification(request.Email);

        var owner = await _walletRepository.FindAsync(spec);
        if (owner == null)
        {
            deleteOwnerResponse.Success = false;
            deleteOwnerResponse.Message = $"You made a Bad Request";

            return deleteOwnerResponse;
        }

        await _walletRepository.DeleteAsync(owner);

        deleteOwnerResponse.Success = true;
        deleteOwnerResponse.Message = $"204 No-Content";

        return deleteOwnerResponse;
    }
}
