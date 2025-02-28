using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Interfaces;
using Wallet.Domain.Events;
using Wallet.Shared.IntegrationEvents;

namespace Wallet.Application.Features.Events.InternalEvents;

public sealed class FundsTransferredDomainEventHandler 
    : INotificationHandler<FundsTransferredDomainEvent>
{
    private readonly ILogger<FundsTransferredDomainEventHandler> _logger;
    private readonly IMassTransitService _massTransitService;

    public FundsTransferredDomainEventHandler(
        ILogger<FundsTransferredDomainEventHandler> logger, 
        IMassTransitService massTransitService)
    {
        _logger = logger;
        _massTransitService = massTransitService;
    }

    public async Task Handle(FundsTransferredDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Successfully consumed event {typeOfEvent} for applicationUsers with Id {Sender} {Receiver} at {time}",
            nameof(FundsTransferredDomainEvent),
            notification.FromWalletEmail,
            notification.ToWalletEmail,
            DateTimeOffset.UtcNow
        );


        await _massTransitService.Publish(new NotifyOwnersOfFundsTransferredEvent(
            notification.FromWalletId,
            notification.FromWalletTransferId,
            notification.FromWalletFirstName,
            notification.FromWalletEmail,
            notification.FromWalletBalance,

            notification.ToWalletId,
            notification.ToWalletTransferId,
            notification.ToWalletFirstName,
            notification.ToWalletEmail,
            notification.ToWalletBalance,

            notification.Amount,
            notification.CreatedAt));

        _logger.LogInformation("Successfully published integration event {typeOfEvent} for applicationUsers with Id {Sender} {Receiver} at {time}",
            nameof(NotifyOwnersOfFundsTransferredEvent),
            notification.FromWalletEmail,
            notification.ToWalletEmail,
            DateTimeOffset.UtcNow
        );
    }
}
