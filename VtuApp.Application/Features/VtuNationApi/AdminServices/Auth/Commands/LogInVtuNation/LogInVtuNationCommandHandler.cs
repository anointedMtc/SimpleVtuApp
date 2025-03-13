using Identity.Shared.Constants;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Exceptions;
using SharedKernel.Application.Interfaces;
using VtuApp.Application.Features.VtuNationApi.AdminServices.Auth.Commands.RegisterWithVtuNationApi;
using VtuApp.Application.Interfaces.ExternalServices.VtuNationApi;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Auth.Commands.LogInVtuNation;

internal sealed class LogInVtuNationCommandHandler : IRequestHandler<LogInVtuNationCommand, LogInVtuNationResponse>
{
    private readonly IGetTokenFromVtuNation _getTokenFromVtuNation;
    private readonly ILogger<LogInVtuNationCommandHandler> _logger;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;

    public LogInVtuNationCommandHandler(IGetTokenFromVtuNation getTokenFromVtuNation, 
        ILogger<LogInVtuNationCommandHandler> logger, IUserContext userContext, 
        IResourceBaseAuthorizationService resourceBaseAuthorizationService)
    {
        _getTokenFromVtuNation = getTokenFromVtuNation;
        _logger = logger;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
    }

    public async Task<LogInVtuNationResponse> Handle(LogInVtuNationCommand request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.GodsEyeOnly))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand?.Email ?? "Anonymous User",
                typeof(LogInVtuNationCommand).Name,
                request);

            throw new ForbiddenAccessException();
        }

        var logInVtuNationResponse = new LogInVtuNationResponse
        {
            LoginResponseVtuNation = new()
        };

        var response = await _getTokenFromVtuNation.GetVtuNationApiTokenAsync(request.LoginRequestVtuNation);

        if (response.IsSuccessful)
        {
            logInVtuNationResponse.LoginResponseVtuNation = response.Content;
            logInVtuNationResponse.Success = true;
            logInVtuNationResponse.Message = $"Successfully Sent RegisterRequest to VtuNationApi";
        }
        else
        {
            _logger.LogError("Unable to process {NameOfRequest} from External Api {Name} at {time} with error message {Error.Message}",
                nameof(LogInVtuNationCommand),
                "VtuNationApi",
                DateTimeOffset.UtcNow,
                response.Error.Message
            );

            // if response is null, it returns an empty list or collection
            logInVtuNationResponse.Success = false;
            //logInVtuNationResponse.Message = $"Error processing your request. Please try again later";
            logInVtuNationResponse.Message = $"---{response.StatusCode}---{response.Error.Message}---{response.Error.InnerException}";
            logInVtuNationResponse.LoginResponseVtuNation = null;
        }

        return logInVtuNationResponse;
    }
}
