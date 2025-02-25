using Identity.Shared.Constants;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Exceptions;
using SharedKernel.Application.Interfaces;
using VtuApp.Application.Interfaces.ExternalServices.VtuNationApi;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Account.Commands.UpdatePasswordVtuNation;

internal sealed class UpdatePasswordVtuNationCommandHandler : IRequestHandler<UpdatePasswordVtuNationCommand, UpdatePasswordVtuNationResponse>
{
    private readonly IGetAdminServicesFromVtuNation _getAdminServicesFromVtuNation;
    private readonly ILogger<UpdatePasswordVtuNationCommandHandler> _logger;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;

    public UpdatePasswordVtuNationCommandHandler(IGetAdminServicesFromVtuNation getAdminServicesFromVtuNation, 
        ILogger<UpdatePasswordVtuNationCommandHandler> logger, 
        IUserContext userContext, IResourceBaseAuthorizationService resourceBaseAuthorizationService)
    {
        _getAdminServicesFromVtuNation = getAdminServicesFromVtuNation;
        _logger = logger;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
    }

    public async Task<UpdatePasswordVtuNationResponse> Handle(UpdatePasswordVtuNationCommand request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.GodsEyeOnly))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand?.Email ?? "Anonymous User",
                typeof(UpdatePasswordVtuNationCommand).Name,
                request);

            throw new ForbiddenAccessException();
        }

        var updatePasswordVtuNationResponse = new UpdatePasswordVtuNationResponse
        {
            UpdatePasswordResponseVtuNation = new()
        };

        var response = await _getAdminServicesFromVtuNation.UpdatePasswordVtuNationAsync(request.UpdatePasswordRequestVtuNation);

        if (response.IsSuccessful)
        {
            updatePasswordVtuNationResponse.UpdatePasswordResponseVtuNation = response.Content;
            updatePasswordVtuNationResponse.Success = true;
            updatePasswordVtuNationResponse.Message = $"Successfully sent UpdatePasswordRequest to VtuNationApi";
        }
        else
        {
            _logger.LogError("Unable to process {NameOfRequest} from External Api {Name} at {time}",
                nameof(UpdatePasswordVtuNationCommand),
                "VtuNationApi",
                DateTimeOffset.UtcNow
            );

            // if response is null, it returns an empty list or collection
            updatePasswordVtuNationResponse.Success = false;
            updatePasswordVtuNationResponse.Message = $"Error processing your request. Please try again later";
            updatePasswordVtuNationResponse.UpdatePasswordResponseVtuNation = null;
        }

        return updatePasswordVtuNationResponse;
    }
}
