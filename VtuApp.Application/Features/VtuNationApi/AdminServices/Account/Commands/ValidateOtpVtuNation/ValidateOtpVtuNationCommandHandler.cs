using Identity.Shared.Constants;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Exceptions;
using SharedKernel.Application.Interfaces;
using VtuApp.Application.Interfaces.ExternalServices.VtuNationApi;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Account.Commands.ValidateOtpVtuNation;

internal sealed class ValidateOtpVtuNationCommandHandler : IRequestHandler<ValidateOtpVtuNationCommand, ValidateOtpVtuNationResponse>
{
    private readonly IGetAdminServicesFromVtuNation _getAdminServicesFromVtuNation;
    private readonly ILogger<ValidateOtpVtuNationCommandHandler> _logger;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;

    public ValidateOtpVtuNationCommandHandler(IGetAdminServicesFromVtuNation getAdminServicesFromVtuNation, 
        ILogger<ValidateOtpVtuNationCommandHandler> logger, IUserContext userContext, 
        IResourceBaseAuthorizationService resourceBaseAuthorizationService)
    {
        _getAdminServicesFromVtuNation = getAdminServicesFromVtuNation;
        _logger = logger;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
    }

    public async Task<ValidateOtpVtuNationResponse> Handle(ValidateOtpVtuNationCommand request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.GodsEyeOnly))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand?.Email ?? "Anonymous User",
                typeof(ValidateOtpVtuNationCommand).Name,
                request);

            throw new ForbiddenAccessException();
        }

        var validateOtpVtuNationResponse = new ValidateOtpVtuNationResponse
        {
            ValidateOtpResponseVtuNation = new()
        };

        var response = await _getAdminServicesFromVtuNation.ValidateOtpVtuNationAsync(request.ValidateOtpRequestVtuNation);

        if (response.IsSuccessful)
        {
            validateOtpVtuNationResponse.ValidateOtpResponseVtuNation = response.Content;
            validateOtpVtuNationResponse.Success = true;
            validateOtpVtuNationResponse.Message = $"Successfully sent ValidateOtpRequest to VtuNationApi";
        }
        else
        {
            _logger.LogError("Unable to process {NameOfRequest} from External Api {Name} at {time} with error message {Error.Message}",
                nameof(ValidateOtpVtuNationCommand),
                "VtuNationApi",
                DateTimeOffset.UtcNow,
                response.Error.Message
            );

            // if response is null, it returns an empty list or collection
            validateOtpVtuNationResponse.Success = false;
            validateOtpVtuNationResponse.Message = $"---{response.StatusCode}---{response.Error.Message}---{response.Error.InnerException}";
            validateOtpVtuNationResponse.ValidateOtpResponseVtuNation = null;
        }

        return validateOtpVtuNationResponse;
    }
}
