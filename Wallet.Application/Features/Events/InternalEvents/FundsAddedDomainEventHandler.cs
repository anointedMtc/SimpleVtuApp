using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Interfaces;
using SharedKernel.Common.Constants;
using Wallet.Domain.Events;
using Wallet.Shared.IntegrationEvents;

namespace Wallet.Application.Features.Events.InternalEvents;

public sealed class FundsAddedDomainEventHandler : INotificationHandler<FundsAddedDomainEvent>
{
    private readonly ILogger<FundsTransferredDomainEventHandler> _logger;
    private readonly IMassTransitService _massTransitService;

    public FundsAddedDomainEventHandler(ILogger<FundsTransferredDomainEventHandler> logger, 
        IMassTransitService massTransitService)
    {
        _logger = logger;
        _massTransitService = massTransitService;
    }

    public async Task Handle(FundsAddedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Successfully consumed event {typeOfEvent} for applicationUser with Id {Email} at {time}",
            nameof(FundsAddedDomainEvent),
            notification.Email,
            DateTimeOffset.UtcNow
        );

        // NotifyOwnerOfFundsAddedEvent
        await _massTransitService.Send(QueueConstants.AddVtuAppBalanceMessageQueueName,
            new AddVtuAppBalanceMessage(
            notification.WalletId,
            notification.OwnerId,
            notification.ApplicationUserId,
            notification.Email,
            //notification.FirstName,
            notification.ReasonWhy,
            notification.TransferId,
            notification.Amount,
            notification.FinalBalance,
            notification.CreatedAt));

        _logger.LogInformation("Successfully published integration event {typeOfEvent} for applicationUser with Id {Email} at {time}",
            nameof(AddVtuAppBalanceMessage),
            notification.Email,
            DateTimeOffset.UtcNow
        );
    }
}
