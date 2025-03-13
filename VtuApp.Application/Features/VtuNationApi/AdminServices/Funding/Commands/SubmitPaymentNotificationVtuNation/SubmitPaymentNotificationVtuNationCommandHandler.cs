using Identity.Shared.Constants;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Exceptions;
using SharedKernel.Application.Interfaces;
using VtuApp.Application.Interfaces.ExternalServices.VtuNationApi;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Funding.Commands.SubmitPaymentNotificationVtuNation;

internal sealed class SubmitPaymentNotificationVtuNationCommandHandler : IRequestHandler<SubmitPaymentNotificationVtuNationCommand, SubmitPaymentNotificationVtuNationResponse>
{
    private readonly IGetAdminServicesFromVtuNation _getAdminServicesFromVtuNation;
    private readonly ILogger<SubmitPaymentNotificationVtuNationCommandHandler> _logger;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;

    public SubmitPaymentNotificationVtuNationCommandHandler(IGetAdminServicesFromVtuNation getAdminServicesFromVtuNation, 
        ILogger<SubmitPaymentNotificationVtuNationCommandHandler> logger, 
        IUserContext userContext, IResourceBaseAuthorizationService resourceBaseAuthorizationService)
    {
        _getAdminServicesFromVtuNation = getAdminServicesFromVtuNation;
        _logger = logger;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
    }

    public async Task<SubmitPaymentNotificationVtuNationResponse> Handle(SubmitPaymentNotificationVtuNationCommand request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.GodsEyeOnly))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand?.Email ?? "Anonymous User",
                typeof(SubmitPaymentNotificationVtuNationCommand).Name,
                request);

            throw new ForbiddenAccessException();
        }

        var submitPaymentNotificationVtuNationResponse = new SubmitPaymentNotificationVtuNationResponse
        {
            SubmitPaymentNotificationResponseVtuNation = new()
        };

        var response = await _getAdminServicesFromVtuNation.SubmitPaymentNotificationVtuNationAsync(request.SubmitPaymentNotificationRequestVtuNation);

        if (response.IsSuccessful)
        {
            submitPaymentNotificationVtuNationResponse.SubmitPaymentNotificationResponseVtuNation = response.Content;
            submitPaymentNotificationVtuNationResponse.Success = true;
            submitPaymentNotificationVtuNationResponse.Message = $"Successfully Sent SubmitPaymentNotification Request to VtuNationApi";
        }
        else
        {
            _logger.LogError("Unable to process {NameOfRequest} from External Api {Name} at {time} with error message {Error.Message}",
                nameof(SubmitPaymentNotificationVtuNationCommand),
                "VtuNationApi",
                DateTimeOffset.UtcNow,
                response.Error.Message
            );

            // if response is null, it returns an empty list or collection
            submitPaymentNotificationVtuNationResponse.Success = false;
            submitPaymentNotificationVtuNationResponse.Message = $"---{response.StatusCode}---{response.Error.Message}---{response.Error.InnerException}";
            submitPaymentNotificationVtuNationResponse.SubmitPaymentNotificationResponseVtuNation = null;
        }

        return submitPaymentNotificationVtuNationResponse;
    }
}
