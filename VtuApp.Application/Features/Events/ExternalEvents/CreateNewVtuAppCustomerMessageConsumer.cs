using MassTransit;
using Microsoft.Extensions.Logging;
using SharedKernel.Domain.Interfaces;
using VtuApp.Application.Exceptions;
using VtuApp.Domain.Entities.VtuModelAggregate;
using VtuApp.Domain.Specifications;
using VtuApp.Shared.IntegrationEvents;

namespace VtuApp.Application.Features.Events.ExternalEvents;

public sealed class CreateNewVtuAppCustomerMessageConsumer : IConsumer<CreateNewVtuAppCustomerMessage>
{
    private readonly ILogger<CreateNewVtuAppCustomerMessageConsumer> _logger;
    private readonly IRepository<Customer> _customerRepository;

    public CreateNewVtuAppCustomerMessageConsumer(ILogger<CreateNewVtuAppCustomerMessageConsumer> logger,
        IRepository<Customer> customerRepository)
    {
        _logger = logger;
        _customerRepository = customerRepository;
    }

    public async Task Consume(ConsumeContext<CreateNewVtuAppCustomerMessage> context)
    {
        _logger.LogInformation("Successfully consumed event {typeOfEvent} for applicationUser with Id {applicationUserId} at {time}",
            nameof(CreateNewVtuAppCustomerMessage),
            context.Message.UserEmail,
            DateTimeOffset.UtcNow
        );

        var spec = new GetCustomerByEmailSpecification(context.Message.UserEmail);

        if (await _customerRepository.FindAsync(spec) is not null)
        {
            _logger.LogError("Tried to create customer with Id {ownerEmail} that already exists",
                context.Message.UserEmail);

            throw new CustomerAlreadyExistsException(context.Message.UserEmail);
        }

        var customer = new Customer(
            context.Message.ApplicationUserId,
            context.Message.UserFirstName,
            context.Message.UserLastName,
            context.Message.UserEmail,
            context.Message.PhoneNumber,
            context.Message.RegisterationBonus);

        await _customerRepository.AddAsync(customer);

        _logger.LogInformation("Created a VtuAppCustomer for the user with ID: {userId}.",
            context.Message.UserEmail);

    }
}
