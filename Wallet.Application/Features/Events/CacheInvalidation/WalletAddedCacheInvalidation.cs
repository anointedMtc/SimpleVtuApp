using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Interfaces;
using Wallet.Domain.Events;

namespace Wallet.Application.Features.Events.CacheInvalidation;

internal sealed class WalletAddedCacheInvalidation : INotificationHandler<WalletAddedDomainEvent>
{
    private readonly ICacheServiceRedis _cacheServiceRedis;
    private readonly ILogger<WalletAddedCacheInvalidation> _logger;

    public WalletAddedCacheInvalidation(ICacheServiceRedis cacheServiceRedis, 
        ILogger<WalletAddedCacheInvalidation> logger)
    {
        _cacheServiceRedis = cacheServiceRedis;
        _logger = logger;
    }

    public async Task Handle(WalletAddedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("invalidating cache for key: {CacheKey} from cache.", 
            notification.Wallet.ApplicationUserId
        );


        await _cacheServiceRedis.RemoveAsync(notification.Wallet.ApplicationUserId.ToString(), cancellationToken);
    }
}
