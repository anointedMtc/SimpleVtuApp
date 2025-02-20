using ApplicationSharedKernel.Exceptions;
using ApplicationSharedKernel.Interfaces;
using Identity.Shared.Constants;
using MediatR;
using Microsoft.Extensions.Logging;
using VtuApp.Application.Interfaces.ExternalServices.VtuNationApi;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Auth.Queries.GetProfileVtuNation;

internal sealed class GetProfileVtuNationQueryHandler : IRequestHandler<GetProfileVtuNationQuery, GetProfileVtuNationResponse>
{
    private readonly IGetAdminServicesFromVtuNation _getAdminServicesFromVtuNation;
    private readonly ILogger<GetProfileVtuNationQueryHandler> _logger;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;

    public GetProfileVtuNationQueryHandler(IGetAdminServicesFromVtuNation getAdminServicesFromVtuNation, 
        ILogger<GetProfileVtuNationQueryHandler> logger, IUserContext userContext, 
        IResourceBaseAuthorizationService resourceBaseAuthorizationService)
    {
        _getAdminServicesFromVtuNation = getAdminServicesFromVtuNation;
        _logger = logger;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
    }

    public async Task<GetProfileVtuNationResponse> Handle(GetProfileVtuNationQuery request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.GodsEyeOnly))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand?.Email ?? "Anonymous User",
                typeof(GetProfileVtuNationQuery).Name,
                request);

            throw new ForbiddenAccessException();
        }

        var getProfileVtuNationResponse = new GetProfileVtuNationResponse
        {
            GetProfileResponseVtuNation = new()
        };

        var response = await _getAdminServicesFromVtuNation.GetProfileVtuNationAsync();

        if (response.IsSuccessful)
        {
            getProfileVtuNationResponse.GetProfileResponseVtuNation = response.Content;
            getProfileVtuNationResponse.Success = true;
            getProfileVtuNationResponse.Message = $"Successfully Sent GetProfile Request to VtuNationApi";
        }
        else
        {
            _logger.LogError("Unable to process {NameOfRequest} from External Api {Name} at {time}",
                nameof(GetProfileVtuNationQuery),
                "VtuNationApi",
                DateTimeOffset.UtcNow
            );

            // if response is null, it returns an empty list or collection
            getProfileVtuNationResponse.Success = false;
            getProfileVtuNationResponse.Message = $"Error processing your request. Please try again later";
            getProfileVtuNationResponse.GetProfileResponseVtuNation = null;
        }

        return getProfileVtuNationResponse;
    }
}
