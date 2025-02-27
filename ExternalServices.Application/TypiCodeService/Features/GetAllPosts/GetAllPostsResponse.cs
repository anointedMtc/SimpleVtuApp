using ExternalServices.Application.TypiCodeService.Models;
using SharedKernel.Application.DTO;

namespace ExternalServices.Application.TypiCodeService.Features.GetAllPosts;

public class GetAllPostsResponse : ApiBaseResponse
{
    public List<TypiCodePost>? TypiCodePosts { get; set; }
}
