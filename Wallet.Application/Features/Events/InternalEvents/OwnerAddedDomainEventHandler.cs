﻿//using MediatR;
//using Microsoft.Extensions.Logging;
//using SharedKernel.Domain.Interfaces;
//using Wallet.Application.Exceptions;
//using Wallet.Domain.Entities.WalletAggregate;
//using Wallet.Domain.Events;
//using Wallet.Domain.Interfaces;
//using Wallet.Domain.Specifications;

//namespace Wallet.Application.Features.Events.InternalEvents;

//public class OwnerAddedDomainEventHandler : INotificationHandler<OwnerAddedDomainEvent>
//{
//    private readonly IWalletRepository<WalletDomainEntity> _walletRepository;
//    private readonly ILogger<OwnerAddedDomainEventHandler> _logger;

//    public OwnerAddedDomainEventHandler(IWalletRepository<WalletDomainEntity> walletRepository,
//        ILogger<OwnerAddedDomainEventHandler> logger)
//    {
//        _walletRepository = walletRepository;
//        _logger = logger;
//    }

//    public async Task Handle(OwnerAddedDomainEvent notification, CancellationToken cancellationToken)
//    {
//        // the goal here is to create a new wallet for the new user/owner
//        var spec = new GetWalletDomainEntityByOwnerIdSpecification(notification.OwnerId);
//        if (await _walletRepository.FindAsync(spec) is not null)
//        {
//            _logger.LogError("Tried to create new Wallet for WalletOwner with Id {WalletOwnerId} who already has an existing wallet",
//                notification.OwnerId);

//            throw new WalletAlreadyExistsException(notification.OwnerEmail);
//        }

//        var wallet = new WalletDomainEntity(notification.OwnerId, notification.ApplicationUser, notification.OwnerEmail);

//        await _walletRepository.AddAsync(wallet);

//        _logger.LogInformation("Created a new wallet for the user/owner with ID: {OwnerId}.",
//            notification.OwnerId);

//    }
//}
