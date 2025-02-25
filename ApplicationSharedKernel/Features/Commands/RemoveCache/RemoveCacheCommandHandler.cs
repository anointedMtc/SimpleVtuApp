using Identity.Shared.Constants;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Interfaces;

namespace SharedKernel.Application.Features.Commands.RemoveCache;

internal sealed class RemoveCacheCommandHandler : IRequestHandler<RemoveCacheCommand, RemoveCacheResponse>
{
    private readonly ICacheServiceRedis _cacheServiceRedis;
    private readonly ILogger<RemoveCacheCommandHandler> _logger;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;

    public RemoveCacheCommandHandler(ICacheServiceRedis cacheServiceRedis, 
        ILogger<RemoveCacheCommandHandler> logger, IUserContext userContext, 
        IResourceBaseAuthorizationService resourceBaseAuthorizationService)
    {
        _cacheServiceRedis = cacheServiceRedis;
        _logger = logger;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
    }

    public async Task<RemoveCacheResponse> Handle(RemoveCacheCommand request, CancellationToken cancellationToken)
    {
        var removeCacheResponse = new RemoveCacheResponse();

        var userExecutingCommand = _userContext.GetCurrentUser();

        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.AdminAndAbove))
        {
            _logger.LogWarning("User with id {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand!.Email ?? "Anonymous User",
                typeof(RemoveCacheCommand),
                request
            );

            //throw new CustomForbiddenException();
            removeCacheResponse.Success = false;
            removeCacheResponse.Message = "Forbidden. You are not allowed to access this resource";

            return removeCacheResponse;
        }

        _logger.LogInformation("invalidating cache for key: {CacheKey} from cache, by {Admin}",
            request.CacheKey,
            userExecutingCommand!.Email
        );

        await _cacheServiceRedis.RemoveAsync(request.CacheKey, cancellationToken);
        //await _cacheServiceRedis.RemoveAsync( request.CacheKey, cancellationToken ).ConfigureAwait( false );

        removeCacheResponse.Success = true;
        removeCacheResponse.Message = $"Successfully removed cache for key {request.CacheKey} from cache repository";

        return removeCacheResponse;
    }
}
