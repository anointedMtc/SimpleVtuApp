using ExternalServices.Application.TypiCodeService.Features.GetAllPosts;
using ExternalServices.Application.TypiCodeService.Features.GetPostById;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Api.Controllers;

namespace ExternalServices.Api.Controllers.TypiCodeService;

[ApiController]

public class TypiCodeController : ApiBaseController
{
    [HttpGet("get-all-posts")]
    public async Task<ActionResult<GetAllPostsResponse>> GetAllTypicodePosts()
    {
        var result = await Mediator.Send(new GetAllPostsQuery());

        return Ok(result);
    }


    [HttpGet("get-post-by-id/{id}")]
    public async Task<ActionResult<GetPostByIdResponse>> GetById(int id)
    {
        // because of the absence of () after the GetPostsByIdQuery what follows becomes an anonymous object
        var result = await Mediator.Send(new GetPostByIdQuery { Id = id });

        return Ok(result);
    }
}
