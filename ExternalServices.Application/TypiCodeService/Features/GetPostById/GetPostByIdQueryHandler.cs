using ExternalServices.Application.TypiCodeService.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ExternalServices.Application.TypiCodeService.Features.GetPostById;

public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, GetPostByIdResponse>
{
    private readonly IGetServiceFromExternalApi _getServiceFromExternalApi;
    private readonly ILogger<GetPostByIdQueryHandler> _logger;

    public GetPostByIdQueryHandler(IGetServiceFromExternalApi getServiceFromExternalApi, 
        ILogger<GetPostByIdQueryHandler> logger)
    {
        _getServiceFromExternalApi = getServiceFromExternalApi;
        _logger = logger;
    }

    public async Task<GetPostByIdResponse> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        var getPostByIdResponse = new GetPostByIdResponse();

        var response = await _getServiceFromExternalApi.GetPostAsync(request.Id);

        if (response.IsSuccessful)
        {
            _logger.LogInformation("successfully retireved post for Id {postId}", request.Id);

            getPostByIdResponse.TypiCodePost = response.Content;
            getPostByIdResponse.Success = true;
            getPostByIdResponse.Message = $"Successfully retrieved the post with id {request.Id}";
        }
        else
        {
            getPostByIdResponse.Success = false;
            getPostByIdResponse.Message = $"Bad Request";
        }

        return getPostByIdResponse;
    }
}
