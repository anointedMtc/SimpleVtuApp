using Identity.Shared.Constants;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Exceptions;
using SharedKernel.Application.Interfaces;
using VtuApp.Application.Interfaces.ExternalServices.VtuNationApi;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Transactions.Queries.GetSingleTransactionVtuNation;

internal sealed class GetSingleTransactionVtuNationQueryHandler : IRequestHandler<GetSingleTransactionVtuNationQuery, GetSingleTransactionVtuNationResponse>
{
    private readonly IGetAdminServicesFromVtuNation _getAdminServicesFromVtuNation;
    private readonly ILogger<GetSingleTransactionVtuNationQueryHandler> _logger;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;

    public GetSingleTransactionVtuNationQueryHandler(IGetAdminServicesFromVtuNation getAdminServicesFromVtuNation, 
        ILogger<GetSingleTransactionVtuNationQueryHandler> logger, 
        IUserContext userContext, IResourceBaseAuthorizationService resourceBaseAuthorizationService)
    {
        _getAdminServicesFromVtuNation = getAdminServicesFromVtuNation;
        _logger = logger;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
    }

    public async Task<GetSingleTransactionVtuNationResponse> Handle(GetSingleTransactionVtuNationQuery request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.GodsEyeOnly))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand?.Email ?? "Anonymous User",
                typeof(GetSingleTransactionVtuNationQuery).Name,
                request);

            throw new ForbiddenAccessException();
        }

        var getSingleTransactionVtuNationResponse = new GetSingleTransactionVtuNationResponse
        {
            GetSingleTransactionResponseVtuNation = new()
        };

        var response = await _getAdminServicesFromVtuNation.GetSingleTransactionVtuNationAsync(request.Id);

        if (response.IsSuccessful)
        {
            getSingleTransactionVtuNationResponse.GetSingleTransactionResponseVtuNation = response.Content;
            getSingleTransactionVtuNationResponse.Success = true;
            getSingleTransactionVtuNationResponse.Message = $"Successfully Sent GetSingleTransaction Request to VtuNationApi";
        }
        else
        {
            _logger.LogError("Unable to process {NameOfRequest} from External Api {Name} at {time}",
                nameof(GetSingleTransactionVtuNationQuery),
                "VtuNationApi",
                DateTimeOffset.UtcNow
            );

            // if response is null, it returns an empty list or collection
            getSingleTransactionVtuNationResponse.Success = false;
            getSingleTransactionVtuNationResponse.Message = $"Error processing your request. Please try again later";
            getSingleTransactionVtuNationResponse.GetSingleTransactionResponseVtuNation = null;
        }

        return getSingleTransactionVtuNationResponse;
    }
}
