using MassTransit;
using Microsoft.Extensions.Logging;
using SagaOrchestrationStateMachines.Shared.IntegrationEvents.VtuAirtimeSaga;
using SharedKernel.Application.Exceptions;
using SharedKernel.Domain.Interfaces;
using VtuApp.Domain.Entities.VtuModelAggregate;
using VtuApp.Domain.Interfaces;
using VtuApp.Domain.Specifications;
using VtuApp.Shared.Constants;
using VtuApp.Shared.IntegrationEvents;

namespace VtuApp.Application.Features.Events.ExternalEvents;

public sealed class RollbackAmountForVtuAirtimePurchaseFailedMessageConsumer
    : IConsumer<RollbackAmountForVtuAirtimePurchaseFailedMessage>
{
    private readonly IVtuAppRepository<Customer> _customerRepository;
    private readonly ILogger<RollbackAmountForVtuAirtimePurchaseFailedMessageConsumer> _logger;

    public RollbackAmountForVtuAirtimePurchaseFailedMessageConsumer(
        IVtuAppRepository<Customer> customerRepository, 
        ILogger<RollbackAmountForVtuAirtimePurchaseFailedMessageConsumer> logger)
    {
        _customerRepository = customerRepository;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<RollbackAmountForVtuAirtimePurchaseFailedMessage> context)
    {
        _logger.LogInformation("Successfully consumed event {typeOfEvent} {typeOfConsumer} for applicationUser with Id {applicationUserId} and transactionId {transactionId} at {time}",
           nameof(RollbackAmountForVtuAirtimePurchaseFailedMessage),
           nameof(RollbackAmountForVtuAirtimePurchaseFailedMessageConsumer),
           context.Message.Email,
           context.Message.VtuTransactionId,
           DateTimeOffset.UtcNow
        );

        var spec = new GetCustomerByEmailSpecification(context.Message.Email);

        var vtuCustomer = await _customerRepository.FindAsync(spec);
        if (vtuCustomer is null)
        {
            _logger.LogError("Tried to process {typeOfEvent} by {typeOfConsumer} for a customer that does not exist {customerId} at {time} with request {@Details}",
                nameof(RollbackAmountForVtuAirtimePurchaseFailedMessage),
                nameof(RollbackAmountForVtuAirtimePurchaseFailedMessageConsumer),
                context.Message.Email,
                DateTimeOffset.UtcNow,
                context.Message
            );

            throw new NotFoundException();
        }

        vtuCustomer.AddToCustomerBalance(context.Message.PricePaid);

        await _customerRepository.UpdateAsync(vtuCustomer);

        await context.Publish(new FundsRefundedForVtuPurchaseFailureEvent(
            context.Message.ApplicationUserId,
            context.Message.Email,
            context.Message.FirstName,
            context.Message.VtuTransactionId,
            context.Message.NetworkProvider,
            TypeOfTransaction.Airtime,
            null,
            context.Message.AmountPurchased,
            context.Message.PricePaid,
            context.Message.Reciever,
            context.Message.CreatedAt)
        );

        _logger.LogInformation("Successfully published {typeOfEvent} from {nameOfPublisher} for customer {customerId} with transaction Id {transactionId} at {time}",
            nameof(FundsRefundedForVtuPurchaseFailureEvent),
            nameof(RollbackAmountForVtuAirtimePurchaseFailedMessageConsumer),
            context.Message.Email,
            context.Message.VtuTransactionId,
            DateTimeOffset.UtcNow
        );
    }
}
