using Identity.Shared.Constants;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Interfaces;
using SharedKernel.Domain.Interfaces;
using Wallet.Domain.Entities.WalletAggregate;

namespace Wallet.Application.Features.Commands.AddFunds;

public class AddFundsCommandHandler : IRequestHandler<AddFundsCommand, AddFundsResponse>
{
    private readonly IRepository<WalletDomainEntity> _walletRepository;
    private readonly ILogger<AddFundsCommandHandler> _logger;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;
    private readonly IMassTransitService _massTransitService;

    public AddFundsCommandHandler(IRepository<WalletDomainEntity> walletRepository,
        ILogger<AddFundsCommandHandler> logger, IUserContext userContext,
        IResourceBaseAuthorizationService resourceBaseAuthorizationService, 
        IMassTransitService massTransitService)
    {
        _walletRepository = walletRepository;
        _logger = logger;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
        _massTransitService = massTransitService;
    }

    public async Task<AddFundsResponse> Handle(AddFundsCommand request, CancellationToken cancellationToken)
    {
        var addFundsResponse = new AddFundsResponse();

        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.AdminAndAbove))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand?.Email ?? "Anonymous User",
                nameof(AddFundsCommand),
                request);

            //throw new ForbiddenAccessException();
            addFundsResponse.Success = false;
            addFundsResponse.Message = $"You are not authorized to access this endpoint.";

            return addFundsResponse;
        }

        var wallet = await _walletRepository.GetByIdAsync(request.WalletId);
        if (wallet == null)
        {
            addFundsResponse.Success = false;
            addFundsResponse.Message = $"You made a Bad Request";

            return addFundsResponse;
        }

        var transfer = wallet.AddFunds(request.Amount, request.ReasonWhy);

        await _walletRepository.UpdateAsync(wallet);

        _logger.LogInformation("Added {Amount} to the wallet: {WalletId} because of {reason}",
            transfer.Amount,
            transfer.WalletDomainEntityId,
            transfer.ReasonWhy
        );


        addFundsResponse.Success = true;
        addFundsResponse.Message = $"204 No Content";

        return addFundsResponse;
    }
}
