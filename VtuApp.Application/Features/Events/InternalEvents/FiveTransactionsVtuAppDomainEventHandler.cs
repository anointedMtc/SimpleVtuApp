using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Interfaces;
using VtuApp.Domain.Events;
using VtuApp.Shared.IntegrationEvents;

namespace VtuApp.Application.Features.Events.InternalEvents;

public sealed class FiveTransactionsVtuAppDomainEventHandler
    : INotificationHandler<FiveTransactionsVtuAppDomainEvent>
{
    private readonly ILogger<FiveTransactionsVtuAppDomainEventHandler> _logger;
    private readonly IMassTransitService _massTransitService;

    public FiveTransactionsVtuAppDomainEventHandler(
        ILogger<FiveTransactionsVtuAppDomainEventHandler> logger, 
        IMassTransitService massTransitService)
    {
        _logger = logger;
        _massTransitService = massTransitService;
    }

    public async Task Handle(FiveTransactionsVtuAppDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Successfully consumed event {typeOfEvent} for applicationUser with Id {applicationUserId} at {time}",
            nameof(FiveTransactionsVtuAppDomainEvent),
            notification.Email,
            DateTimeOffset.UtcNow
        );

        await _massTransitService.Publish(new NotifyUserOfFiveVtuTransactionsAchievedEvent(
            notification.CustomerId,
            notification.Email,
            notification.FirstName,
            notification.BonusForFiveTransactions,
            notification.FinalVtuBonusBalance,
            notification.CreatedAt));

        _logger.LogInformation("Successfully published integration event {typeOfEvent} for applicationUser with Id {applicationUserId} at {time}",
           nameof(NotifyUserOfFiveVtuTransactionsAchievedEvent),
           notification.Email,
           DateTimeOffset.UtcNow
        );
    }
}
