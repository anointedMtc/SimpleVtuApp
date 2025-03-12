using Identity.Shared.Constants;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Exceptions;
using SharedKernel.Application.Interfaces;
using VtuApp.Application.Interfaces.ExternalServices.VtuNationApi;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Funding.Queries.GetFundNotificationsVtuNation;

internal sealed class GetFundNotificationsVtuNationQueryHandler : IRequestHandler<GetFundNotificationsVtuNationQuery, GetFundNotificationsVtuNationResponse>
{
    private readonly IGetAdminServicesFromVtuNation _getAdminServicesFromVtuNation;
    private readonly ILogger<GetFundNotificationsVtuNationQueryHandler> _logger;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;

    public GetFundNotificationsVtuNationQueryHandler(IGetAdminServicesFromVtuNation getAdminServicesFromVtuNation, 
        ILogger<GetFundNotificationsVtuNationQueryHandler> logger, 
        IUserContext userContext, IResourceBaseAuthorizationService resourceBaseAuthorizationService)
    {
        _getAdminServicesFromVtuNation = getAdminServicesFromVtuNation;
        _logger = logger;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
    }

    public async Task<GetFundNotificationsVtuNationResponse> Handle(GetFundNotificationsVtuNationQuery request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.GodsEyeOnly))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand?.Email ?? "Anonymous User",
                typeof(GetFundNotificationsVtuNationQuery).Name,
                request);

            throw new ForbiddenAccessException();
        }

        var getFundNotificationsVtuNationResponse = new GetFundNotificationsVtuNationResponse
        {
            GetFundNotificationsResponseVtuNation = new()
        };

        var response = await _getAdminServicesFromVtuNation.GetFundNotificationsVtuNationAsync();

        if (response.IsSuccessful)
        {
            getFundNotificationsVtuNationResponse.GetFundNotificationsResponseVtuNation = response.Content;
            getFundNotificationsVtuNationResponse.Success = true;
            getFundNotificationsVtuNationResponse.Message = $"Successfully Sent GetFundNotification Request to VtuNationApi";
        }
        else
        {
            _logger.LogError("Unable to process {NameOfRequest} from External Api {Name} at {time} with error message {Error.Message}",
                nameof(GetFundNotificationsVtuNationQuery),
                "VtuNationApi",
                DateTimeOffset.UtcNow,
                response.Error.Message
            );

            // if response is null, it returns an empty list or collection
            getFundNotificationsVtuNationResponse.Success = false;
            getFundNotificationsVtuNationResponse.Message = $"---{response.StatusCode}---{response.Error.Message}---{response.Error.InnerException}";
            getFundNotificationsVtuNationResponse.GetFundNotificationsResponseVtuNation = null;
        }

        return getFundNotificationsVtuNationResponse;
    }
}
