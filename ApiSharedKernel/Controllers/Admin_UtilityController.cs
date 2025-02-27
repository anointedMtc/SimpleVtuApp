using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Application.Features.Commands.RemoveCache;

namespace SharedKernel.Api.Controllers;

[ApiVersion("1.0")]
public sealed class Admin_UtilityController : ApiBaseController
{
    [HttpPost("remove-cache")]
    [Authorize]
    public async Task<ActionResult<RemoveCacheResponse>> RemoveCache([FromQuery] string cacheKey)
    {
        var result = await Mediator.Send(new RemoveCacheCommand() { CacheKey = cacheKey });

        return Ok(result);
    }
}
