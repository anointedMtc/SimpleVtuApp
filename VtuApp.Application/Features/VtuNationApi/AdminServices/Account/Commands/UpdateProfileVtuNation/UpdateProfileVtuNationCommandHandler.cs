using Identity.Shared.Constants;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Exceptions;
using SharedKernel.Application.Interfaces;
using VtuApp.Application.Interfaces.ExternalServices.VtuNationApi;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Account.Commands.UpdateProfileVtuNation;

internal sealed class UpdateProfileVtuNationCommandHandler : IRequestHandler<UpdateProfileVtuNationCommand, UpdateProfileVtuNationResponse>
{
    private readonly IGetAdminServicesFromVtuNation _getAdminServicesFromVtuNation;
    private readonly ILogger<UpdateProfileVtuNationCommandHandler> _logger;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;

    public UpdateProfileVtuNationCommandHandler(IGetAdminServicesFromVtuNation getAdminServicesFromVtuNation, 
        ILogger<UpdateProfileVtuNationCommandHandler> logger, IUserContext userContext, 
        IResourceBaseAuthorizationService resourceBaseAuthorizationService)
    {
        _getAdminServicesFromVtuNation = getAdminServicesFromVtuNation;
        _logger = logger;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
    }

    public async Task<UpdateProfileVtuNationResponse> Handle(UpdateProfileVtuNationCommand request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.GodsEyeOnly))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand?.Email ?? "Anonymous User",
                typeof(UpdateProfileVtuNationCommand).Name,
                request);

            throw new ForbiddenAccessException();
        }

        var updateProfileVtuNationResponse = new UpdateProfileVtuNationResponse
        {
            UpdateProfileResponseVtuNation = new()
        };

        var response = await _getAdminServicesFromVtuNation.UpdateProfileVtuNationAsync(request.UpdateProfileRequestVtuNation);

        if (response.IsSuccessful)
        {
            updateProfileVtuNationResponse.UpdateProfileResponseVtuNation = response.Content;
            updateProfileVtuNationResponse.Success = true;
            updateProfileVtuNationResponse.Message = $"Successfully sent UpdateProfileRequest to VtuNationApi";
        }
        else
        {
            _logger.LogError("Unable to process {NameOfRequest} from External Api {Name} at {time} with error message {Error.Message}",
                nameof(UpdateProfileVtuNationCommand),
                "VtuNationApi",
                DateTimeOffset.UtcNow,
                response.Error.Message
            );

            // if response is null, it returns an empty list or collection
            updateProfileVtuNationResponse.Success = false;
            updateProfileVtuNationResponse.Message = $"---{response.StatusCode}---{response.Error.Message}---{response.Error.InnerException}";
            updateProfileVtuNationResponse.UpdateProfileResponseVtuNation = null;
        }

        return updateProfileVtuNationResponse;
    }
}
