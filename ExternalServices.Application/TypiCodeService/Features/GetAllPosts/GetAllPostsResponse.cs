using ApplicationSharedKernel.DTO;
using ExternalServices.Application.TypiCodeService.Interfaces;

namespace ExternalServices.Application.TypiCodeService.Features.GetAllPosts;

public class GetAllPostsResponse : ApiBaseResponse
{
    public List<TypiCodePost>? TypiCodePosts { get; set; }
}
