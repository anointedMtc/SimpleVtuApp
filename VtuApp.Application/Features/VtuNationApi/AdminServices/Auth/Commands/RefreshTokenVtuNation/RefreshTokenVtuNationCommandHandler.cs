using Identity.Shared.Constants;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Exceptions;
using SharedKernel.Application.Interfaces;
using VtuApp.Application.Interfaces.ExternalServices.VtuNationApi;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Auth.Commands.RefreshTokenVtuNation;

internal sealed class RefreshTokenVtuNationCommandHandler : IRequestHandler<RefreshTokenVtuNationCommand, RefreshTokenVtuNationResponse>
{
    private readonly IGetAdminServicesFromVtuNation _getAdminServicesFromVtuNation;
    private readonly ILogger<RefreshTokenVtuNationCommandHandler> _logger;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;

    public RefreshTokenVtuNationCommandHandler(IGetAdminServicesFromVtuNation getAdminServicesFromVtuNation, 
        ILogger<RefreshTokenVtuNationCommandHandler> logger, IUserContext userContext, 
        IResourceBaseAuthorizationService resourceBaseAuthorizationService)
    {
        _getAdminServicesFromVtuNation = getAdminServicesFromVtuNation;
        _logger = logger;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
    }

    public async Task<RefreshTokenVtuNationResponse> Handle(RefreshTokenVtuNationCommand request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.GodsEyeOnly))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand?.Email ?? "Anonymous User",
                typeof(RefreshTokenVtuNationCommand).Name,
                request);

            throw new ForbiddenAccessException();
        }

        var refreshTokenVtuNationResponse = new RefreshTokenVtuNationResponse
        {
            RefreshTokenResponseVtuNation = new()
        };

        var response = await _getAdminServicesFromVtuNation.RefreshTokenVtuNationAsync();

        if (response.IsSuccessful)
        {
            refreshTokenVtuNationResponse.RefreshTokenResponseVtuNation = response.Content;
            refreshTokenVtuNationResponse.Success = true;
            refreshTokenVtuNationResponse.Message = $"Successfully Sent RefreshTokenRequest to VtuNationApi";
        }
        else
        {
            _logger.LogError("Unable to process {NameOfRequest} from External Api {Name} at {time}",
                nameof(RefreshTokenVtuNationCommand),
                "VtuNationApi",
                DateTimeOffset.UtcNow
            );

            // if response is null, it returns an empty list or collection
            refreshTokenVtuNationResponse.Success = false;
            refreshTokenVtuNationResponse.Message = $"Error processing your request. Please try again later";
            refreshTokenVtuNationResponse.RefreshTokenResponseVtuNation = null;
        }

        return refreshTokenVtuNationResponse;
    }
}
