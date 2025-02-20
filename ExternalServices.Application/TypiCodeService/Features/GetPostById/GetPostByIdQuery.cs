using MediatR;

namespace ExternalServices.Application.TypiCodeService.Features.GetPostById;

public class GetPostByIdQuery : IRequest<GetPostByIdResponse>
{
    public int Id { get; set; }
}
