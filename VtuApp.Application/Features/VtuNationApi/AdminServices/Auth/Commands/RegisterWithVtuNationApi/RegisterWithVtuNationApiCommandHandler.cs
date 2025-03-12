using Identity.Shared.Constants;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Exceptions;
using SharedKernel.Application.Interfaces;
using VtuApp.Application.Interfaces.ExternalServices.VtuNationApi;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Auth.Commands.RegisterWithVtuNationApi;

internal sealed class RegisterWithVtuNationApiCommandHandler : IRequestHandler<RegisterWithVtuNationApiCommand, RegisterWithVtuNationApiResponse>
{
    private readonly IGetAdminServicesFromVtuNation _getAdminServicesFromVtuNation;
    private readonly ILogger<RegisterWithVtuNationApiCommandHandler> _logger;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;

    public RegisterWithVtuNationApiCommandHandler(IGetAdminServicesFromVtuNation getAdminServicesFromVtuNation, 
        ILogger<RegisterWithVtuNationApiCommandHandler> logger, IUserContext userContext, 
        IResourceBaseAuthorizationService resourceBaseAuthorizationService)
    {
        _getAdminServicesFromVtuNation = getAdminServicesFromVtuNation;
        _logger = logger;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
    }

    public async Task<RegisterWithVtuNationApiResponse> Handle(RegisterWithVtuNationApiCommand request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.GodsEyeOnly))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand?.Email ?? "Anonymous User",
                typeof(RegisterWithVtuNationApiCommand).Name,
                request);

            throw new ForbiddenAccessException();
        }

        var registerWithVtuNationApiResponse = new RegisterWithVtuNationApiResponse
        {
            RegisterResponseVtuNation = new()
        };

        var response = await _getAdminServicesFromVtuNation.RegisterWithVtuNationApiAsync(request.RegisterRequestVtuNation);

        if (response.IsSuccessful)
        {
            registerWithVtuNationApiResponse.RegisterResponseVtuNation = response.Content;
            registerWithVtuNationApiResponse.Success = true;
            registerWithVtuNationApiResponse.Message = $"Successfully Sent RegisterRequest to VtuNationApi";
        }
        else
        {
            _logger.LogError("Unable to process {NameOfRequest} from External Api {Name} at {time} with error message {Error.Message}",
                nameof(RegisterWithVtuNationApiCommand),
                "VtuNationApi",
                DateTimeOffset.UtcNow,
                response.Error.Message
            );

            // if response is null, it returns an empty list or collection
            registerWithVtuNationApiResponse.Success = false;
            registerWithVtuNationApiResponse.Message = $"---{response.StatusCode}---{response.Error.Message}---{response.Error.InnerException}";
            registerWithVtuNationApiResponse.RegisterResponseVtuNation = null;
        }

        return registerWithVtuNationApiResponse;
    }
}
