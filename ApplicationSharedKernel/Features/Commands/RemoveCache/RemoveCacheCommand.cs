using MediatR;

namespace SharedKernel.Application.Features.Commands.RemoveCache;

public sealed class RemoveCacheCommand : IRequest<RemoveCacheResponse>
{
    public string CacheKey { get; set; }

}
