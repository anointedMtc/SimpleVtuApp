using ApplicationSharedKernel.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using VtuApp.Domain.Events;
using VtuApp.Shared.IntegrationEvents;

namespace VtuApp.Application.Features.Events.InternalEvents;

public class VtuAppCustomerCreatedDomainEventHandler : INotificationHandler<VtuAppCustomerCreatedDomainEvent>
{
    private readonly ILogger<VtuAppCustomerCreatedDomainEventHandler> _logger;
    private readonly IMassTransitService _massTransitService;

    public VtuAppCustomerCreatedDomainEventHandler(ILogger<VtuAppCustomerCreatedDomainEventHandler> logger, 
        IMassTransitService massTransitService)
    {
        _logger = logger;
        _massTransitService = massTransitService;
    }

    public async Task Handle(VtuAppCustomerCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Successfully consumed event {typeOfEvent} for applicationUser with Id {applicationUserId} at {time}",
            nameof(VtuAppCustomerCreatedDomainEvent),
            notification.Email,
            DateTimeOffset.UtcNow
        );

        await _massTransitService.Publish(new VtuAppCustomerCreatedIntegrationEvent(
            notification.ApplicationUserId,
            notification.Email)
        );

        _logger.LogInformation("Successfully published integration event {typeOfEvent} for applicationUser with Id {applicationUserId} at {time}",
           nameof(VtuAppCustomerCreatedDomainEvent),
           notification.Email,
           DateTimeOffset.UtcNow
        );
    }
}
