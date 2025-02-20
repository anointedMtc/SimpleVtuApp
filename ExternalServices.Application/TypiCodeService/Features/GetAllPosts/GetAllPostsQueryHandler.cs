using ExternalServices.Application.TypiCodeService.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ExternalServices.Application.TypiCodeService.Features.GetAllPosts;

public class GetAllPostsQueryHandler : IRequestHandler<GetAllPostsQuery, GetAllPostsResponse>
{
    private readonly IGetServiceFromExternalApi _getServiceFromExternalApi;
    private readonly ILogger<GetAllPostsQueryHandler> _logger;

    public GetAllPostsQueryHandler(IGetServiceFromExternalApi getServiceFromExternalApi, 
        ILogger<GetAllPostsQueryHandler> logger)
    {
        _getServiceFromExternalApi = getServiceFromExternalApi;
        _logger = logger;
    }

    public async Task<GetAllPostsResponse> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
    {
        var getAllPostsResponse = new GetAllPostsResponse();
        getAllPostsResponse.TypiCodePosts = new List<TypiCodePost>();

        var result = new List<TypiCodePost>();

        var response = await _getServiceFromExternalApi.GetAllPostsAsync();

        if (response.IsSuccessful)
        {
            result = response.Content;
        }
        else
        {
            _logger.LogError("Error getting succesful response from external api client");
        }

        // if result is null, it returns an empty list or collection
        getAllPostsResponse.TypiCodePosts = result;
        getAllPostsResponse.Success = true;
        getAllPostsResponse.Message = $"Successfull retrieved all posts";

        return getAllPostsResponse;
    }
}
