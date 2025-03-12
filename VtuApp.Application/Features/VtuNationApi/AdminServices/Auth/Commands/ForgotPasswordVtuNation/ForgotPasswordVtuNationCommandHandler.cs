using Identity.Shared.Constants;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Exceptions;
using SharedKernel.Application.Interfaces;
using VtuApp.Application.Interfaces.ExternalServices.VtuNationApi;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Auth.Commands.ForgotPasswordVtuNation;

internal sealed class ForgotPasswordVtuNationCommandHandler : IRequestHandler<ForgotPasswordVtuNationCommand, ForgotPasswordVtuNationResponse>
{
    private readonly IGetAdminServicesFromVtuNation _getAdminServicesFromVtuNation;
    private readonly ILogger<ForgotPasswordVtuNationCommandHandler> _logger;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;

    public ForgotPasswordVtuNationCommandHandler(IGetAdminServicesFromVtuNation getAdminServicesFromVtuNation, 
        ILogger<ForgotPasswordVtuNationCommandHandler> logger, IUserContext userContext, 
        IResourceBaseAuthorizationService resourceBaseAuthorizationService)
    {
        _getAdminServicesFromVtuNation = getAdminServicesFromVtuNation;
        _logger = logger;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
    }

    public async Task<ForgotPasswordVtuNationResponse> Handle(ForgotPasswordVtuNationCommand request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.GodsEyeOnly))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand?.Email ?? "Anonymous User",
                typeof(ForgotPasswordVtuNationCommand).Name,
                request);

            throw new ForbiddenAccessException();
        }

        var forgotPasswordVtuNationResponse = new ForgotPasswordVtuNationResponse
        {
            ForgotPasswordResponseVtuNation = new()
        };

        var response = await _getAdminServicesFromVtuNation.ForgotPasswordVtuNationAsync(request.ForgotPasswordRequestVtuNation);

        if (response.IsSuccessful)
        {
            forgotPasswordVtuNationResponse.ForgotPasswordResponseVtuNation = response.Content;
            forgotPasswordVtuNationResponse.Success = true;
            forgotPasswordVtuNationResponse.Message = $"Successfully sent ForgotPasswordRequest to VtuNationApi";
        }
        else
        {
            _logger.LogError("Unable to process {NameOfRequest} from External Api {Name} at {time} with error message {Error.Message}",
                nameof(ForgotPasswordVtuNationCommand),
                "VtuNationApi",
                DateTimeOffset.UtcNow,
                response.Error.Message
            );

            // if response is null, it returns an empty list or collection
            forgotPasswordVtuNationResponse.Success = false;
            forgotPasswordVtuNationResponse.Message = $"---{response.StatusCode}---{response.Error.Message}---{response.Error.InnerException}";
            forgotPasswordVtuNationResponse.ForgotPasswordResponseVtuNation = null;
        }

        return forgotPasswordVtuNationResponse;
    }
}
