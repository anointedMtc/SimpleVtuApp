using Identity.Shared.Constants;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Exceptions;
using SharedKernel.Application.Interfaces;
using VtuApp.Application.Features.VtuNationApi.AdminServices.Account.Commands.GenerateOtpVtuNation;
using VtuApp.Application.Interfaces.ExternalServices.VtuNationApi;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Account.Commands.SendEmailVerificationLinkVtuNation;

internal sealed class SendEmailVerificationLinkVtuNationCommandHandler : IRequestHandler<SendEmailVerificationLinkVtuNationCommand, SendEmailVerificationLinkVtuNationResponse>
{
    private readonly IGetAdminServicesFromVtuNation _getAdminServicesFromVtuNation;
    private readonly ILogger<GenerateOtpVtuNationCommandHandler> _logger;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;

    public SendEmailVerificationLinkVtuNationCommandHandler(IGetAdminServicesFromVtuNation getAdminServicesFromVtuNation, 
        ILogger<GenerateOtpVtuNationCommandHandler> logger, IUserContext userContext, 
        IResourceBaseAuthorizationService resourceBaseAuthorizationService)
    {
        _getAdminServicesFromVtuNation = getAdminServicesFromVtuNation;
        _logger = logger;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
    }

    public async Task<SendEmailVerificationLinkVtuNationResponse> Handle(SendEmailVerificationLinkVtuNationCommand request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.GodsEyeOnly))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand?.Email ?? "Anonymous User",
                typeof(SendEmailVerificationLinkVtuNationCommand).Name,
                request);

            throw new ForbiddenAccessException();
        }

        var sendEmailVerificationLinkVtuNationResponse = new SendEmailVerificationLinkVtuNationResponse
        {
            SendEmailVerificationLinkResponseVtuNation = new()
        };

        var response = await _getAdminServicesFromVtuNation.SendEmailVerificationLinkVtuNationAsync();

        if (response.IsSuccessful)
        {
            sendEmailVerificationLinkVtuNationResponse.SendEmailVerificationLinkResponseVtuNation = response.Content;
            sendEmailVerificationLinkVtuNationResponse.Success = true;
            sendEmailVerificationLinkVtuNationResponse.Message = $"Successfully sent SendEmailVerificationLink Request to VtuNationApi";
        }
        else
        {
            _logger.LogError("Unable to process {NameOfRequest} from External Api {Name} at {time} with error message {Error.Message}",
                nameof(SendEmailVerificationLinkVtuNationCommand),
                "VtuNationApi",
                DateTimeOffset.UtcNow,
                response.Error.Message
            );

            // if response is null, it returns an empty list or collection
            sendEmailVerificationLinkVtuNationResponse.Success = false;
            sendEmailVerificationLinkVtuNationResponse.Message = $"---{response.StatusCode}---{response.Error.Message}---{response.Error.InnerException}";
            sendEmailVerificationLinkVtuNationResponse.SendEmailVerificationLinkResponseVtuNation = null;
        }

        return sendEmailVerificationLinkVtuNationResponse;
    }
}
