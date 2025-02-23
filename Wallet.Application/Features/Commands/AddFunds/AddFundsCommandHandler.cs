using DomainSharedKernel.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Wallet.Domain.Entities.WalletAggregate;

namespace Wallet.Application.Features.Commands.AddFunds;

public class AddFundsCommandHandler : IRequestHandler<AddFundsCommand, AddFundsResponse>
{
    private readonly IRepository<WalletDomainEntity> _walletRepository;
    private readonly ILogger<AddFundsCommandHandler> _logger;

    public AddFundsCommandHandler(IRepository<WalletDomainEntity> walletRepository,
        ILogger<AddFundsCommandHandler> logger)
    {
        _walletRepository = walletRepository;
        _logger = logger;
    }

    public async Task<AddFundsResponse> Handle(AddFundsCommand request, CancellationToken cancellationToken)
    {
        var addFundsResponse = new AddFundsResponse();

        var wallet = await _walletRepository.GetByIdAsync(request.WalletId);
        if (wallet == null)
        {
            addFundsResponse.Success = false;
            addFundsResponse.Message = $"You made a Bad Request";

            return addFundsResponse;
        }

        var transfer = wallet.AddFunds(request.Amount, request.ReasonWhy);

        await _walletRepository.UpdateAsync(wallet);

        addFundsResponse.Success = true;
        addFundsResponse.Message = $"204 No Content";

        _logger.LogInformation("Added {Amount} to the wallet: {WalletId} because of {reason}",
            transfer.Amount,
            transfer.WalletDomainEntityId,
            transfer.ReasonWhy
        );

        return addFundsResponse;

    }
}
