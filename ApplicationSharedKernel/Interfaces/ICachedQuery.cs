using MediatR;

namespace SharedKernel.Application.Interfaces;

public interface ICachedQuery<TResponse> : IRequest<TResponse>, ICachedQuery;

public interface ICachedQuery
{
    string CacheKey { get; }

    TimeSpan? Expiration { get; }  // we are leaving it nullable incase we don't want to specify and in that case the default expiration time is going to apply
}