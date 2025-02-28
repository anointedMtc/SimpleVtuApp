using MassTransit;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Exceptions;
using SharedKernel.Application.Interfaces;
using VtuApp.Shared.IntegrationEvents;
using Wallet.Domain.Entities.WalletAggregate;
using Wallet.Domain.Interfaces;
using Wallet.Domain.Specifications;
using Wallet.Shared.IntegrationEvents;

namespace Wallet.Application.Features.Events.ExternalEvents;

public sealed class VtuBonusTransferredToWalletEventConsumer
    : IConsumer<VtuBonusTransferredToWalletEvent>
{
    private readonly ILogger<VtuBonusTransferredToWalletEventConsumer> _logger;
    private readonly IWalletRepository<WalletDomainEntity> _walletRepository;
    private readonly IMassTransitService _massTransitService;

    public VtuBonusTransferredToWalletEventConsumer(
        ILogger<VtuBonusTransferredToWalletEventConsumer> logger, 
        IWalletRepository<WalletDomainEntity> walletRepository,
        IMassTransitService massTransitService)
    {
        _logger = logger;
        _walletRepository = walletRepository;
        _massTransitService = massTransitService;
    }

    public async Task Consume(ConsumeContext<VtuBonusTransferredToWalletEvent> context)
    {
        _logger.LogInformation("Successfully consumed event {typeOfEvent} {typeOfConsumer} for applicationUser with Id {applicationUserId} and transactionId {transactionId} at {time}",
            nameof(VtuBonusTransferredToWalletEvent),
            nameof(VtuBonusTransferredToWalletEventConsumer),
            context.Message.Email,
            context.Message.VtuBonusTransferId,
            DateTimeOffset.UtcNow
        );

        var spec = new GetWalletDomainEntityByEmailSpecification(context.Message.Email);

        var wallet = await _walletRepository.FindAsync(spec);

        if (wallet is null)
        {
            _logger.LogError("Tried to process {typeOfEvent} by {typeOfConsumer} for a customerWallet that does not exist {customerId} at {time} with request {@Details}",
                nameof(VtuBonusTransferredToWalletEvent),
                nameof(VtuBonusTransferredToWalletEventConsumer),
                context.Message.Email,
                DateTimeOffset.UtcNow,
                context.Message
            );

            throw new NotFoundException();
        }

        wallet.AddFunds(context.Message.AmountTransferred, $"vtuBonusTransfer: {context.Message.VtuBonusTransferId}");

        await _walletRepository.UpdateAsync(wallet);

        _logger.LogInformation("Successfully processed {typeOfRequest} by {typeOfConsumer} for Customer with Id {customerId} and transactionId {transactionId} at {time} with External Api details {@Response}",
            nameof(VtuBonusTransferredToWalletEvent),
            nameof(VtuBonusTransferredToWalletEventConsumer),
            context.Message.Email,
            context.Message.VtuBonusTransferId,
            DateTimeOffset.UtcNow,
            context.Message
        );


        await _massTransitService.Publish(new NotifyUserOfVtuBonusTransferedToWalletEvent(
            wallet.ApplicationUserId,
            wallet.Email,
            //wallet.Owner.FirstName,
            context.Message.FirstName,
            context.Message.VtuBonusTransferId,
            context.Message.AmountTransferred,
            context.Message.InitialBonusBalance,
            context.Message.FinalBonusBalance,
            context.Message.CreatedAt));

        _logger.LogInformation("Successfully published {typeOfEvent} from {nameOfPublisher} for customer {customerId} with transaction Id {vtuBonusTransferId} at {time}",
            nameof(NotifyUserOfVtuBonusTransferedToWalletEvent),
            nameof(VtuBonusTransferredToWalletEventConsumer),
            context.Message.Email,
            context.Message.VtuBonusTransferId,
            DateTimeOffset.UtcNow
        );

    }
}
