using ApplicationSharedKernel.DTO;
using ExternalServices.Application.TypiCodeService.Interfaces;

namespace ExternalServices.Application.TypiCodeService.Features.GetPostById;

public class GetPostByIdResponse : ApiBaseResponse
{
    public TypiCodePost TypiCodePost { get; set; }
}
