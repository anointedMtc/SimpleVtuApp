using DomainSharedKernel.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Wallet.Domain.Entities.WalletAggregate;

namespace Wallet.Application.Features.Commands.DeductFunds;

public class DeductFundsCommandHandler : IRequestHandler<DeductFundsCommand, DeductFundsResponse>
{
    private readonly IRepository<WalletDomainEntity> _walletRepository;
    private readonly ILogger<DeductFundsCommandHandler> _logger;

    public DeductFundsCommandHandler(IRepository<WalletDomainEntity> walletRepository,
        ILogger<DeductFundsCommandHandler> logger)
    {
        _walletRepository = walletRepository;
        _logger = logger;
    }

    public async Task<DeductFundsResponse> Handle(DeductFundsCommand request, CancellationToken cancellationToken)
    {
        var deductFundsResponse = new DeductFundsResponse();

        var wallet = await _walletRepository.GetByIdAsync(request.WalletId);
        if (wallet == null)
        {
            deductFundsResponse.Success = false;
            deductFundsResponse.Message = $"You made a Bad Request";

            return deductFundsResponse;
        }

        var transfer = wallet.DeductFunds(request.Amount, request.ReasonWhy);

        await _walletRepository.UpdateAsync(wallet);

        deductFundsResponse.Success = true;
        deductFundsResponse.Message = $"204 No Content";

        _logger.LogInformation("Added {transferAmount} to the wallet with Id: {WalletId} because of {reason}",
            transfer.Amount,
            wallet.WalletDomainEntityId, 
            transfer.ReasonWhy
        );

        return deductFundsResponse;
    }
}
