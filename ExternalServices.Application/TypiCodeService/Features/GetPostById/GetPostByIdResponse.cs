using ExternalServices.Application.TypiCodeService.Models;
using SharedKernel.Application.DTO;

namespace ExternalServices.Application.TypiCodeService.Features.GetPostById;

public class GetPostByIdResponse : ApiBaseResponse
{
    public TypiCodePost TypiCodePost { get; set; }
}
