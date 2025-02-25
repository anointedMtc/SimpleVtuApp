using MassTransit;
using Microsoft.Extensions.Logging;
using SagaOrchestrationStateMachines.Shared.IntegrationEvents.VtuAirtimeSaga;
using SharedKernel.Application.Exceptions;
using SharedKernel.Domain.Interfaces;
using VtuApp.Application.Interfaces.ExternalServices.VtuNationApi;
using VtuApp.Domain.Entities.VtuModelAggregate;
using VtuApp.Domain.Specifications;
using VtuApp.Shared.DTO.VtuNationApi.UserServices;
using VtuApp.Shared.IntegrationEvents;

namespace VtuApp.Application.Features.Events.ExternalEvents;

public sealed class SecondRetryVtuAirtimeOrderEventConsumer : IConsumer<SecondRetryVtuAirtimeOrderEvent>
{
    private readonly IRepository<Customer> _customerRepository;
    private readonly ILogger<SecondRetryVtuAirtimeOrderEventConsumer> _logger;
    private readonly IGetServicesFromVtuNation _getServicesFromVtuNation;

    public SecondRetryVtuAirtimeOrderEventConsumer(IRepository<Customer> customerRepository, 
        ILogger<SecondRetryVtuAirtimeOrderEventConsumer> logger, 
        IGetServicesFromVtuNation getServicesFromVtuNation)
    {
        _customerRepository = customerRepository;
        _logger = logger;
        _getServicesFromVtuNation = getServicesFromVtuNation;
    }

    public async Task Consume(ConsumeContext<SecondRetryVtuAirtimeOrderEvent> context)
    {
        _logger.LogInformation("Successfully consumed event {typeOfEvent} by {typeOfConsumer} for applicationUser with Id {applicationUserId} and transactionId {transactionId} at {time}",
            nameof(SecondRetryVtuAirtimeOrderEvent),
            nameof(SecondRetryVtuAirtimeOrderEventConsumer),
            context.Message.Email,
            context.Message.VtuTransactionId,
            DateTimeOffset.UtcNow
        );

        var spec = new GetCustomerByEmailSpecification(context.Message.Email);

        if (await _customerRepository.FindAsync(spec) is null)
        {
            _logger.LogError("Tried to process {typeOfEvent} by {typeOfEventConsumer} for a customer that does not exist {customerId} at {time} with request {@Details}",
                nameof(SecondRetryVtuAirtimeOrderEvent),
                nameof(SecondRetryVtuAirtimeOrderEventConsumer),
                context.Message.Email,
                DateTimeOffset.UtcNow,
                context.Message);

            throw new NotFoundException();
        }

        var buyAirtimeRequestDto = new BuyAirtimeRequestVtuNation
        {
            Amount = context.Message.AmountToPurchase,
            Network = context.Message.NetworkProvider.ToString(),
            MobileNumber = context.Message.Reciever
        };

        var response = await _getServicesFromVtuNation.BuyAirtimeVtuNationAsync(buyAirtimeRequestDto);

        if (response.IsSuccessful)
        {
            _logger.LogInformation("Successfully processed {typeOfRequest} by {typeOfConsumer} for Customer with Id {customerId} and transactionId {transactionId} at {time} with External Api details {@Response}",
                nameof(SecondRetryVtuAirtimeOrderEvent),
                nameof(SecondRetryVtuAirtimeOrderEventConsumer),
                context.Message.Email,
                context.Message.VtuTransactionId,
                DateTimeOffset.UtcNow,
                response.Content
            );

            await context.Publish(new BuyAirtimeForCustomerSuccessEvent(
                context.Message.ApplicationUserId,
                context.Message.Email,
                context.Message.VtuTransactionId,
                context.Message.NetworkProvider,
                context.Message.AmountToPurchase,
                context.Message.Reciever)
            );

            _logger.LogInformation("Successfully published {typeOfEvent} from {nameOfPublisher} for customer {customerId} with transaction Id {transactionId} at {time}",
                nameof(BuyAirtimeForCustomerSuccessEvent),
                nameof(SecondRetryVtuAirtimeOrderEventConsumer),
                context.Message.Email,
                context.Message.VtuTransactionId,
                DateTimeOffset.UtcNow
            );
        }
        else
        {
            _logger.LogError("Unable to process {typeOfRequest} by {typeOfConsumer} for Customer with Id {customerId} and transactionId {transactionId} at {time} with External Api details {@Response}",
                nameof(SecondRetryVtuAirtimeOrderEvent),
                nameof(SecondRetryVtuAirtimeOrderEventConsumer),
                context.Message.Email,
                context.Message.VtuTransactionId,
                DateTimeOffset.UtcNow,
                response.Content
            );

            await context.Publish(new BuyAirtimeForCustomerSecondReTryFailedEvent(
                context.Message.ApplicationUserId,
                context.Message.Email,
                context.Message.VtuTransactionId,
                context.Message.NetworkProvider,
                context.Message.AmountToPurchase,
                context.Message.Reciever)
            );

            _logger.LogInformation("Successfully published {typeOfEvent} from {nameOfPublisher} for customer {customerId} with transaction Id {transactionId} at {time}",
                nameof(BuyAirtimeForCustomerSecondReTryFailedEvent),
                nameof(SecondRetryVtuAirtimeOrderEventConsumer),
                context.Message.Email,
                context.Message.VtuTransactionId,
                DateTimeOffset.UtcNow
            );
        }
    }
}
