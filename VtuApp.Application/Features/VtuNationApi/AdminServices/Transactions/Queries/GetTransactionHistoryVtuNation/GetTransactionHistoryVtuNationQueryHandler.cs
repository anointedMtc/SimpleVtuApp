using Identity.Shared.Constants;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Exceptions;
using SharedKernel.Application.Interfaces;
using VtuApp.Application.Interfaces.ExternalServices.VtuNationApi;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Transactions.Queries.GetTransactionHistoryVtuNation;

internal sealed class GetTransactionHistoryVtuNationQueryHandler : IRequestHandler<GetTransactionHistoryVtuNationQuery, GetTransactionHistoryVtuNationResponse>
{
    private readonly IGetAdminServicesFromVtuNation _getAdminServicesFromVtuNation;
    private readonly ILogger<GetTransactionHistoryVtuNationQueryHandler> _logger;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;

    public GetTransactionHistoryVtuNationQueryHandler(IGetAdminServicesFromVtuNation getAdminServicesFromVtuNation, 
        ILogger<GetTransactionHistoryVtuNationQueryHandler> logger, 
        IUserContext userContext, IResourceBaseAuthorizationService resourceBaseAuthorizationService)
    {
        _getAdminServicesFromVtuNation = getAdminServicesFromVtuNation;
        _logger = logger;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
    }

    public async Task<GetTransactionHistoryVtuNationResponse> Handle(GetTransactionHistoryVtuNationQuery request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.GodsEyeOnly))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand?.Email ?? "Anonymous User",
                typeof(GetTransactionHistoryVtuNationQuery).Name,
                request);

            throw new ForbiddenAccessException();
        }

        var getTransactionHistoryVtuNationResponse = new GetTransactionHistoryVtuNationResponse
        {
            GetTransactionHistoryResponseVtuNation = new()
        };

        var response = await _getAdminServicesFromVtuNation.GetTransactionHistoryVtuNationAsync();

        if (response.IsSuccessful)
        {
            getTransactionHistoryVtuNationResponse.GetTransactionHistoryResponseVtuNation = response.Content;
            getTransactionHistoryVtuNationResponse.Success = true;
            getTransactionHistoryVtuNationResponse.Message = $"Successfully Sent GetTransactionHistory Request to VtuNationApi";
        }
        else
        {
            _logger.LogError("Unable to process {NameOfRequest} from External Api {Name} at {time}",
                nameof(GetTransactionHistoryVtuNationQuery),
                "VtuNationApi",
                DateTimeOffset.UtcNow
            );

            // if response is null, it returns an empty list or collection
            getTransactionHistoryVtuNationResponse.Success = false;
            getTransactionHistoryVtuNationResponse.Message = $"Error processing your request. Please try again later";
            getTransactionHistoryVtuNationResponse.GetTransactionHistoryResponseVtuNation = null;
        }

        return getTransactionHistoryVtuNationResponse;
    }
}
