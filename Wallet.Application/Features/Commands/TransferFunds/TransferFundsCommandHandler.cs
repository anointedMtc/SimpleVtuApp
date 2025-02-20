using DomainSharedKernel.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Wallet.Domain.Entities.WalletAggregate;

namespace Wallet.Application.Features.Commands.TransferFunds;

public class TransferFundsCommandHandler : IRequestHandler<TransferFundsCommand, TransferFundsResponse>
{
    private readonly IRepository<WalletDomainEntity> _walletRepository;
    private readonly ILogger<TransferFundsCommandHandler> _logger;

    public TransferFundsCommandHandler(IRepository<WalletDomainEntity> walletRepository,
        ILogger<TransferFundsCommandHandler> logger)
    {
        _walletRepository = walletRepository;
        _logger = logger;
    }

    public async Task<TransferFundsResponse> Handle(TransferFundsCommand request, CancellationToken cancellationToken)
    {
        var transferFundsResponse = new TransferFundsResponse();

        var fromWallet = await _walletRepository.GetByIdAsync(request.FromWalletId);
        if (fromWallet is null)
        {
            transferFundsResponse.Success = false;
            transferFundsResponse.Message = $"Bad Request";

            return transferFundsResponse;
        }

        var toWallet = await _walletRepository.GetByIdAsync(request.ToWalletId);
        if (toWallet is null)
        {
            transferFundsResponse.Success = false;
            transferFundsResponse.Message = $"Bad Request";

            return transferFundsResponse;
        }

        fromWallet.TransferFunds(toWallet, request.Amount, request.ReasonWhy);

        await _walletRepository.UpdateAsync(fromWallet);
        await _walletRepository.UpdateAsync(toWallet);
        _logger.LogInformation("Transferred {Amount} from: {FromWalletId} to: {ToWalletId} because of {reason}.",
            request.Amount,
            request.FromWalletId,
            request.ToWalletId,
            request.ReasonWhy
        );

        transferFundsResponse.Success = true;
        transferFundsResponse.Message = $"204 No Content";

        return transferFundsResponse;

    }
}
