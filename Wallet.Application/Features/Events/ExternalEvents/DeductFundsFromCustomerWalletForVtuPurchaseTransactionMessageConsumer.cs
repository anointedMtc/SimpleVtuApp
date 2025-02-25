using MassTransit;
using Microsoft.Extensions.Logging;
using SagaOrchestrationStateMachines.Shared.IntegrationEvents.VtuAirtimeSaga;
using SharedKernel.Application.Exceptions;
using SharedKernel.Domain.Interfaces;
using Wallet.Domain.Entities.WalletAggregate;
using Wallet.Domain.Specifications;

namespace Wallet.Application.Features.Events.ExternalEvents;

public sealed class DeductFundsFromCustomerWalletForVtuPurchaseTransactionMessageConsumer
    : IConsumer<DeductFundsFromCustomerWalletForVtuPurchaseTransactionMessage>
{
    private readonly ILogger<DeductFundsFromCustomerWalletForVtuPurchaseTransactionMessageConsumer> _logger;
    private readonly IRepository<WalletDomainEntity> _walletDomainEntityRepository;
    
    public DeductFundsFromCustomerWalletForVtuPurchaseTransactionMessageConsumer(
        ILogger<DeductFundsFromCustomerWalletForVtuPurchaseTransactionMessageConsumer> logger,
        IRepository<WalletDomainEntity> walletDomainEntityRepository)
    {
        _logger = logger;
        _walletDomainEntityRepository = walletDomainEntityRepository;
    }

    public async Task Consume(ConsumeContext<DeductFundsFromCustomerWalletForVtuPurchaseTransactionMessage> context)
    {
        _logger.LogInformation("Successfully consumed event {typeOfEvent} {typeOfConsumer} for applicationUser with Id {applicationUserId} and transactionId {transactionId} at {time}",
            nameof(DeductFundsFromCustomerWalletForVtuPurchaseTransactionMessage),
            nameof(DeductFundsFromCustomerWalletForVtuPurchaseTransactionMessageConsumer),
            context.Message.Email,
            context.Message.VtuTransactionId,
            DateTimeOffset.UtcNow
        );

        var spec = new GetWalletDomainEntityByEmailSpecification(context.Message.Email);

        var wallet = await _walletDomainEntityRepository.FindAsync(spec);

        if (wallet is null)
        {
            _logger.LogError("Tried to process {typeOfEvent} by {typeOfConsumer} for a customerWallet that does not exist {customerId} at {time} with request {@Details}",
                nameof(DeductFundsFromCustomerWalletForVtuPurchaseTransactionMessage),
                nameof(DeductFundsFromCustomerWalletForVtuPurchaseTransactionMessageConsumer),
                context.Message.Email,
                DateTimeOffset.UtcNow,
                context.Message
            );

            throw new NotFoundException();
        }

        wallet.DeductFunds(context.Message.PricePaid, $"vtuTransaction: {context.Message.VtuTransactionId}");

        await _walletDomainEntityRepository.UpdateAsync(wallet);
       
        _logger.LogInformation("Successfully processed {typeOfRequest} by {typeOfConsumer} for Customer with Id {customerId} and transactionId {transactionId} at {time} with External Api details {@Response}",
                nameof(DeductFundsFromCustomerWalletForVtuPurchaseTransactionMessage),
                nameof(DeductFundsFromCustomerWalletForVtuPurchaseTransactionMessageConsumer),
                context.Message.Email,
                context.Message.VtuTransactionId,
                DateTimeOffset.UtcNow,
                context.Message
        );
    }
}
