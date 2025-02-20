using ApplicationSharedKernel.Exceptions;
using ApplicationSharedKernel.Interfaces;
using Identity.Shared.Constants;
using MediatR;
using Microsoft.Extensions.Logging;
using VtuApp.Application.Interfaces.ExternalServices.VtuNationApi;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Funding.Queries.GetPaymentAccountInfoVtuNation;

internal sealed class GetPaymentAccountInfoVtuNationQueryHandler : IRequestHandler<GetPaymentAccountInfoVtuNationQuery, GetPaymentAccountInfoVtuNationResponse>
{
    private readonly IGetAdminServicesFromVtuNation _getAdminServicesFromVtuNation;
    private readonly ILogger<GetPaymentAccountInfoVtuNationQueryHandler> _logger;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;

    public GetPaymentAccountInfoVtuNationQueryHandler(IGetAdminServicesFromVtuNation getAdminServicesFromVtuNation, 
        ILogger<GetPaymentAccountInfoVtuNationQueryHandler> logger, 
        IUserContext userContext, IResourceBaseAuthorizationService resourceBaseAuthorizationService)
    {
        _getAdminServicesFromVtuNation = getAdminServicesFromVtuNation;
        _logger = logger;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
    }

    public async Task<GetPaymentAccountInfoVtuNationResponse> Handle(GetPaymentAccountInfoVtuNationQuery request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.GodsEyeOnly))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand?.Email ?? "Anonymous User",
                typeof(GetPaymentAccountInfoVtuNationQuery).Name,
                request);

            throw new ForbiddenAccessException();
        }

        var getPaymentAccountInfoVtuNationResponse = new GetPaymentAccountInfoVtuNationResponse
        {
            PaymentAccountInfoResponseVtuNation = new()
        };

        var response = await _getAdminServicesFromVtuNation.GetPaymentAccountInfoVtuNationAsync();

        if (response.IsSuccessful)
        {
            getPaymentAccountInfoVtuNationResponse.PaymentAccountInfoResponseVtuNation = response.Content;
            getPaymentAccountInfoVtuNationResponse.Success = true;
            getPaymentAccountInfoVtuNationResponse.Message = $"Successfully Sent GetPaymentAccountInfo Request to VtuNationApi";
        }
        else
        {
            _logger.LogError("Unable to process {NameOfRequest} from External Api {Name} at {time}",
                nameof(GetPaymentAccountInfoVtuNationQuery),
                "VtuNationApi",
                DateTimeOffset.UtcNow
            );

            // if response is null, it returns an empty list or collection
            getPaymentAccountInfoVtuNationResponse.Success = false;
            getPaymentAccountInfoVtuNationResponse.Message = $"Error processing your request. Please try again later";
            getPaymentAccountInfoVtuNationResponse.PaymentAccountInfoResponseVtuNation = null;
        }

        return getPaymentAccountInfoVtuNationResponse;
    }
}
