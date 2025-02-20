using ApplicationSharedKernel.Exceptions;
using DomainSharedKernel.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;
using SagaOrchestrationStateMachines.Shared.IntegrationEvents.VtuDataSaga;
using VtuApp.Application.Interfaces.ExternalServices.VtuNationApi;
using VtuApp.Domain.Entities.VtuModelAggregate;
using VtuApp.Domain.Specifications;
using VtuApp.Shared.DTO.VtuNationApi.UserServices;
using VtuApp.Shared.IntegrationEvents;

namespace VtuApp.Application.Features.Events.ExternalEvents;

public sealed class BuyDataForCustomerMessageConsumer : IConsumer<BuyDataForCustomerMessage>
{
    private readonly IRepository<Customer> _customerRepository;
    private readonly ILogger<BuyDataForCustomerMessageConsumer> _logger;
    private readonly IGetServicesFromVtuNation _getServicesFromVtuNation;

    public BuyDataForCustomerMessageConsumer(IRepository<Customer> customerRepository, 
        ILogger<BuyDataForCustomerMessageConsumer> logger, 
        IGetServicesFromVtuNation getServicesFromVtuNation)
    {
        _customerRepository = customerRepository;
        _logger = logger;
        _getServicesFromVtuNation = getServicesFromVtuNation;
    }

    public async Task Consume(ConsumeContext<BuyDataForCustomerMessage> context)
    {
        _logger.LogInformation("Successfully consumed event {typeOfEvent} {typeOfConsumer} for applicationUser with Id {applicationUserId} and transactionId {transactionId} at {time}",
            nameof(BuyDataForCustomerMessage),
            nameof(BuyDataForCustomerMessageConsumer),
            context.Message.Email,
            context.Message.VtuTransactionId,
            DateTimeOffset.UtcNow
        );

        var spec = new GetCustomerByEmailSpecification(context.Message.Email);

        if (await _customerRepository.FindAsync(spec) is null)
        {
            _logger.LogError("Tried to process {typeOfEvent} by {typeOfConsumer} for a customer that does not exist {customerId} at {time} with request {@Details}",
                nameof(BuyDataForCustomerMessage),
                nameof(BuyDataForCustomerMessageConsumer),
                context.Message.Email,
                DateTimeOffset.UtcNow,
                context.Message
            );

            throw new NotFoundException();
        }

        var buyDataRequestDto = new BuyDataRequestVtuNation
        {
            DataPlan = context.Message.DataPlanPurchased,
            Network = context.Message.NetworkProvider.ToString(),
            MobileNumber = context.Message.Reciever
        };

        var response = await _getServicesFromVtuNation.BuyDataVtuNationAsync(buyDataRequestDto);

        if (response.IsSuccessful)
        {
            _logger.LogInformation("Successfully processed {typeOfRequest} by {typeOfConsumer} for Customer with Id {customerId} and transactionId {transactionId} at {time} with External Api details {@Response}",
                nameof(BuyDataForCustomerMessage),
                nameof(BuyDataForCustomerMessageConsumer),
                context.Message.Email,
                context.Message.VtuTransactionId,
                DateTimeOffset.UtcNow,
                response.Content
            );

            await context.Publish(new BuyDataForCustomerSuccessEvent(
                context.Message.ApplicationUserId,
                context.Message.Email,
                context.Message.VtuTransactionId,
                context.Message.NetworkProvider,
                context.Message.DataPlanPurchased,
                context.Message.AmountToPurchase,
                context.Message.Reciever)
            );

            _logger.LogInformation("Successfully published {typeOfEvent} from {nameOfPublisher} for customer {customerId} with transaction Id {transactionId} at {time}",
                nameof(BuyDataForCustomerSuccessEvent),
                nameof(BuyDataForCustomerMessageConsumer),
                context.Message.Email,
                context.Message.VtuTransactionId,
                DateTimeOffset.UtcNow
            );
        }
        else
        {
            _logger.LogError("Unable to process {typeOfRequest} by {typeOfConsumer} for Customer with Id {customerId} and transactionId {transactionId} at {time} with External Api details {@Response}",
                nameof(BuyDataForCustomerMessage),
                nameof(BuyDataForCustomerMessageConsumer),
                context.Message.Email,
                context.Message.VtuTransactionId,
                DateTimeOffset.UtcNow,
                response.Content
            );

            await context.Publish(new BuyDataForCustomerFirstTryFailedEvent(
                context.Message.ApplicationUserId,
                context.Message.Email,
                context.Message.VtuTransactionId,
                context.Message.NetworkProvider,
                context.Message.DataPlanPurchased,
                context.Message.AmountToPurchase,
                context.Message.Reciever)
            );

            _logger.LogInformation("Successfully published {typeOfEvent} from {nameOfPublisher} for customer {customerId} with transaction Id {transactionId} at {time}",
                nameof(BuyDataForCustomerFirstTryFailedEvent),
                nameof(BuyDataForCustomerMessageConsumer),
                context.Message.Email,
                context.Message.VtuTransactionId,
                DateTimeOffset.UtcNow
            );
        }
    }
}
