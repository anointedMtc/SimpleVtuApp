using ApplicationSharedKernel.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Wallet.Domain.Events;
using Wallet.Shared.IntegrationEvents;

namespace Wallet.Application.Features.Events.InternalEvents;

public class WalletAddedDomainEventHandler : INotificationHandler<WalletAddedDomainEvent>
{
    private readonly ILogger<WalletAddedDomainEventHandler> _logger;
    private readonly IMassTransitService _massTransitService;

    public WalletAddedDomainEventHandler(ILogger<WalletAddedDomainEventHandler> logger, 
        IMassTransitService massTransitService)
    {
        _logger = logger;
        _massTransitService = massTransitService;
    }

    public async Task Handle(WalletAddedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Successfully consumed event {typeOfEvent} for applicationUser with Id {applicationUserId} at {time}",
            nameof(WalletAddedDomainEvent),
            notification.Wallet.Email,
            DateTimeOffset.UtcNow
        );

        await _massTransitService.Publish(new WalletAddedIntegrationEvent(
            notification.Wallet.ApplicationUserId,
            notification.Wallet.Email)
        );

        _logger.LogInformation("Successfully published integration event {typeOfEvent} for applicationUser with Id {applicationUserId} at {time}",
            nameof(WalletAddedDomainEvent),
            notification.Wallet.Email,
            DateTimeOffset.UtcNow
        );
    }
}
