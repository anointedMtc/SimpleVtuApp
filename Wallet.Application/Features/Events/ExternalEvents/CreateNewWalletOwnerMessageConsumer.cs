using MassTransit;
using Microsoft.Extensions.Logging;
using SagaOrchestrationStateMachines.Shared.IntegrationEvents.VtuDataSaga;
using SharedKernel.Domain.Interfaces;
using Wallet.Application.Exceptions;
using Wallet.Domain.Entities;
using Wallet.Domain.Entities.WalletAggregate;
using Wallet.Domain.Interfaces;
using Wallet.Domain.Specifications;
using Wallet.Shared.IntegrationEvents;

namespace Wallet.Application.Features.Events.ExternalEvents;

public class CreateNewWalletOwnerMessageConsumer : IConsumer<CreateNewWalletOwnerMessage>
{
    private readonly IWalletRepository<Owner> _ownerRepository;
    private readonly IWalletRepository<WalletDomainEntity> _walletRepository;
    private readonly ILogger<CreateNewWalletOwnerMessageConsumer> _logger;

    public CreateNewWalletOwnerMessageConsumer(ILogger<CreateNewWalletOwnerMessageConsumer> logger,
        IWalletRepository<Owner> ownerRepository,
        IWalletRepository<WalletDomainEntity> walletRepository)
    {
        _logger = logger;
        _ownerRepository = ownerRepository;
        _walletRepository = walletRepository;
    }

    public async Task Consume(ConsumeContext<CreateNewWalletOwnerMessage> context)
    {
        _logger.LogInformation("Successfully consumed event {typeOfEvent} by {typeOfConsumer} for applicationUser with Id {applicationUserId} and correlationId {applicationUserId} at {time} with details {@request}",
           nameof(CreateNewWalletOwnerMessage),
           nameof(CreateNewWalletOwnerMessageConsumer),
           context.Message.UserEmail,
           context.Message.ApplicationUserId,
           DateTimeOffset.UtcNow,
           context.Message
        );

        var spec = new GetOwnerByEmailSpecification(context.Message.UserEmail);
        
        if (await _ownerRepository.FindAsync(spec) is not null)
        {
            _logger.LogError("Tried to create owner with Id {ownerEmail} that already exists",
                context.Message.UserEmail);

            throw new OwnerAlreadyExistsException(context.Message.UserEmail);
        }

        var owner = new Owner(context.Message.ApplicationUserId, context.Message.UserEmail, context.Message.UserFirstName, context.Message.UserLastName);
        await _ownerRepository.AddAsync(owner);

        //var walletForOwner = new WalletDomainEntity(owner.OwnerId, context.Message.ApplicationUserId, context.Message.UserEmail);
        var walletForOwner = owner.CreateWalletForThisOwner();
        await _walletRepository.AddAsync(walletForOwner);

        _logger.LogInformation("This command {nameOfCommand} was successful and it Created an owner with Id {ownerId} and a wallet with id {walletId} for the user with ID: {userId}.",
            nameof(CreateNewWalletOwnerMessageConsumer),
            owner.OwnerId,
            walletForOwner.WalletDomainEntityId,
            context.Message.ApplicationUserId);

    }
}
