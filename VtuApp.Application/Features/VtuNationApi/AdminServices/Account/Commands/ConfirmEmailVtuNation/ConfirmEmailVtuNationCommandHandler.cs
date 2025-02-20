using ApplicationSharedKernel.Exceptions;
using ApplicationSharedKernel.Interfaces;
using Identity.Shared.Constants;
using MediatR;
using Microsoft.Extensions.Logging;
using VtuApp.Application.Interfaces.ExternalServices.VtuNationApi;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Account.Commands.ConfirmEmailVtuNation;

internal sealed class ConfirmEmailVtuNationCommandHandler : IRequestHandler<ConfirmEmailVtuNationCommand, ConfirmEmailVtuNationResponse>
{
    private readonly IGetAdminServicesFromVtuNation _getAdminServicesFromVtuNation;
    private readonly ILogger<ConfirmEmailVtuNationCommandHandler> _logger;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;

    public ConfirmEmailVtuNationCommandHandler(IGetAdminServicesFromVtuNation getAdminServicesFromVtuNation, 
        ILogger<ConfirmEmailVtuNationCommandHandler> logger, 
        IUserContext userContext, 
        IResourceBaseAuthorizationService resourceBaseAuthorizationService)
    {
        _getAdminServicesFromVtuNation = getAdminServicesFromVtuNation;
        _logger = logger;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
    }

    public async Task<ConfirmEmailVtuNationResponse> Handle(ConfirmEmailVtuNationCommand request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.GodsEyeOnly))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand?.Email ?? "Anonymous User",
                typeof(ConfirmEmailVtuNationCommand).Name,
                request);

            throw new ForbiddenAccessException();
        }

        var confirmEmailVtuNationResponse = new ConfirmEmailVtuNationResponse
        {
            ConfirmEmailResponseVtuNation = new()
        };

        var response = await _getAdminServicesFromVtuNation.ConfirmEmailVtuNationAsync(request.ConfirmEmailRequestVtuNation);

        if (response.IsSuccessful)
        {
            confirmEmailVtuNationResponse.ConfirmEmailResponseVtuNation = response.Content;
            confirmEmailVtuNationResponse.Success = true;
            confirmEmailVtuNationResponse.Message = $"Successfully sent ConfirmEmail Request to VtuNationApi";
        }
        else
        {
            _logger.LogError("Unable to process {NameOfRequest} from External Api {Name} at {time}",
                nameof(ConfirmEmailVtuNationCommand),
                "VtuNationApi",
                DateTimeOffset.UtcNow
            );

            // if response is null, it returns an empty list or collection
            confirmEmailVtuNationResponse.Success = false;
            confirmEmailVtuNationResponse.Message = $"Error processing your request. Please try again later";
            confirmEmailVtuNationResponse.ConfirmEmailResponseVtuNation = null;
        }

        return confirmEmailVtuNationResponse;
    }
}
