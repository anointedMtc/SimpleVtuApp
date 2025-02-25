using MassTransit;
using Microsoft.Extensions.Logging;
using SharedKernel.Domain.Interfaces;
using Wallet.Application.Exceptions;
using Wallet.Domain.Entities;
using Wallet.Domain.Specifications;
using Wallet.Shared.IntegrationEvents;

namespace Wallet.Application.Features.Events.ExternalEvents;

public class CreateNewWalletOwnerMessageConsumer : IConsumer<CreateNewWalletOwnerMessage>
{
    private readonly IRepository<Owner> _ownerRepository;
    private readonly ILogger<CreateNewWalletOwnerMessageConsumer> _logger;

    public CreateNewWalletOwnerMessageConsumer(ILogger<CreateNewWalletOwnerMessageConsumer> logger, 
        IRepository<Owner> ownerRepository)
    {
        _logger = logger;
        _ownerRepository = ownerRepository;
    }

    public async Task Consume(ConsumeContext<CreateNewWalletOwnerMessage> context)
    {
        var spec = new GetOwnerByEmailSpecification(context.Message.UserEmail);
        
        if (await _ownerRepository.FindAsync(spec) is not null)
        {
            _logger.LogError("Tried to create owner with Id {ownerEmail} that already exists",
                context.Message.UserEmail);

            throw new OwnerAlreadyExistsException(context.Message.UserEmail);
        }

        var owner = new Owner(context.Message.ApplicationUserId, context.Message.UserEmail, context.Message.UserFirstName, context.Message.UserLastName);

        await _ownerRepository.AddAsync(owner);

        _logger.LogInformation("Created an owner for the user with ID: {userId}.",
            context.Message.ApplicationUserId);
    }
}
