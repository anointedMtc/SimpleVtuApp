using MassTransit;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Exceptions;
using SharedKernel.Application.Interfaces;
using VtuApp.Domain.Entities.VtuModelAggregate;
using VtuApp.Domain.Interfaces;
using VtuApp.Domain.Specifications;
using VtuApp.Shared.IntegrationEvents;
using Wallet.Shared.IntegrationEvents;

namespace VtuApp.Application.Features.Events.ExternalEvents.WalletModule;

public sealed class SubtractVtuAppBalanceMessageConsumer
    : IConsumer<SubtractVtuAppBalanceMessage>
{
    private readonly ILogger<SubtractVtuAppBalanceMessageConsumer> _logger;
    private readonly IVtuAppRepository<Customer> _vtuAppRepository;
    private readonly IMassTransitService _massTransitService;

    public SubtractVtuAppBalanceMessageConsumer(ILogger<SubtractVtuAppBalanceMessageConsumer> logger, 
        IVtuAppRepository<Customer> vtuAppRepository, IMassTransitService massTransitService)
    {
        _logger = logger;
        _vtuAppRepository = vtuAppRepository;
        _massTransitService = massTransitService;
    }

    public async Task Consume(ConsumeContext<SubtractVtuAppBalanceMessage> context)
    {
        _logger.LogInformation("Successfully consumed event {typeOfEvent} by {typeOfConsumer} for applicationUser with Id {applicationUserId} and transactionId {transactionId} at {time}",
           nameof(SubtractVtuAppBalanceMessage),
           nameof(SubtractVtuAppBalanceMessageConsumer),
           context.Message.Email,
           context.Message.TransferID,
           DateTimeOffset.UtcNow
        );

        var spec = new GetCustomerByEmailSpecification(context.Message.Email);

        var customer = await _vtuAppRepository.FindAsync(spec);
        if (customer is null)
        {
            _logger.LogError("Tried to process {typeOfEvent} by {typeOfEventConsumer} for a customer that does not exist {customerId} at {time} with request {@Details}",
                nameof(SubtractVtuAppBalanceMessage),
                nameof(SubtractVtuAppBalanceMessageConsumer),
                context.Message.Email,
                DateTimeOffset.UtcNow,
                context.Message
            );

            throw new NotFoundException();
        }

        customer.DeductFromCustomerBalance(context.Message.Amount);
        await _vtuAppRepository.UpdateAsync(customer);

        _logger.LogInformation("Successfully processed {typeOfEvent} for customer with Id {customerId} at {time}",
            nameof(SubtractVtuAppBalanceMessage),
            context.Message.Email,
            DateTimeOffset.UtcNow);

        await _massTransitService.Publish(new NotifyUserOfFundsDeductedEvent(
            context.Message.WalletId,
            context.Message.OwnerId,
            context.Message.ApplicationUserId,
            context.Message.Email,
            customer.FirstName,
            context.Message.ReasonWhy,
            context.Message.TransferID,
            context.Message.Amount,
            context.Message.FinalBalance,
            context.Message.CreatedAt));

        _logger.LogInformation("Successfully published {typeOfEvent} from {nameOfPublisher} for customer {customerId} with transaction Id {transactionId} at {time}",
            nameof(NotifyUserOfFundsDeductedEvent),
            nameof(SubtractVtuAppBalanceMessageConsumer),
            context.Message.Email,
            context.Message.TransferID,
            DateTimeOffset.UtcNow
        );

    }
}
