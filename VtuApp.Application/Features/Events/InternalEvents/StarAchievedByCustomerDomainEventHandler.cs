using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Interfaces;
using VtuApp.Domain.Events;
using VtuApp.Shared.IntegrationEvents;

namespace VtuApp.Application.Features.Events.InternalEvents;

public sealed class StarAchievedByCustomerDomainEventHandler
    : INotificationHandler<StarAchievedByCustomerDomainEvent>
{
    private readonly ILogger<StarAchievedByCustomerDomainEventHandler> _logger;
    private readonly IMassTransitService _massTransitService;

    public StarAchievedByCustomerDomainEventHandler(
        ILogger<StarAchievedByCustomerDomainEventHandler> logger, 
        IMassTransitService massTransitService)
    {
        _logger = logger;
        _massTransitService = massTransitService;
    }

    public async Task Handle(StarAchievedByCustomerDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Successfully consumed event {typeOfEvent} for applicationUser with Id {applicationUserId} at {time}",
            nameof(StarAchievedByCustomerDomainEvent),
            notification.Email,
            DateTimeOffset.UtcNow
        );

        await _massTransitService.Publish(new NotifyUserOfStarAchievedEvent(
            notification.CustomerId,
            notification.Email,
            notification.FirstName,
            notification.DiscountGiven,
            notification.TotalOfTransactionsMade,
            notification.FinalVtuBonusBalance,
            notification.CreatedAt));

        _logger.LogInformation("Successfully published integration event {typeOfEvent} for applicationUser with Id {applicationUserId} at {time}",
           nameof(NotifyUserOfStarAchievedEvent),
           notification.Email,
           DateTimeOffset.UtcNow
        );
    }
}
