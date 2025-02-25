using Identity.Shared.Constants;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Exceptions;
using SharedKernel.Application.Interfaces;
using VtuApp.Application.Interfaces.ExternalServices.VtuNationApi;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Auth.Commands.LogOutVtuNation;

internal sealed class LogOutVtuNationCommandHandler : IRequestHandler<LogOutVtuNationCommand, LogOutVtuNationResponse>
{
    private readonly IGetAdminServicesFromVtuNation _getAdminServicesFromVtuNation;
    private readonly ILogger<LogOutVtuNationCommandHandler> _logger;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;

    public LogOutVtuNationCommandHandler(IGetAdminServicesFromVtuNation getAdminServicesFromVtuNation, 
        ILogger<LogOutVtuNationCommandHandler> logger, IUserContext userContext, 
        IResourceBaseAuthorizationService resourceBaseAuthorizationService)
    {
        _getAdminServicesFromVtuNation = getAdminServicesFromVtuNation;
        _logger = logger;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
    }

    public async Task<LogOutVtuNationResponse> Handle(LogOutVtuNationCommand request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.GodsEyeOnly))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand?.Email ?? "Anonymous User",
                typeof(LogOutVtuNationCommand).Name,
                request);

            throw new ForbiddenAccessException();
        }

        var logOutVtuNationResponse = new LogOutVtuNationResponse
        {
            LogOutResponseVtuNation = new()
        };

        var response = await _getAdminServicesFromVtuNation.LogOutVtuNationAsync();

        if (response.IsSuccessful)
        {
            logOutVtuNationResponse.LogOutResponseVtuNation = response.Content;
            logOutVtuNationResponse.Success = true;
            logOutVtuNationResponse.Message = $"Successfully logged-Out from VtuNationApi";
        }
        else
        {
            _logger.LogError("Unable to process {NameOfRequest} from External Api {Name} at {time}",
                nameof(LogOutVtuNationCommand),
                "VtuNationApi",
                DateTimeOffset.UtcNow
            );

            // if response is null, it returns an empty list or collection
            logOutVtuNationResponse.Success = false;
            logOutVtuNationResponse.Message = $"Error processing your request. Please try again later";
            logOutVtuNationResponse.LogOutResponseVtuNation = null;
        }

        return logOutVtuNationResponse;
    }
}
