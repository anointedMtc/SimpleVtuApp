﻿using ApplicationSharedKernel.Exceptions;
using ApplicationSharedKernel.Interfaces;
using Identity.Shared.Constants;
using MediatR;
using Microsoft.Extensions.Logging;
using VtuApp.Application.Interfaces.ExternalServices.VtuNationApi;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Earnings.Queries.GetEarningsHistoryVtuNation;

internal sealed class GetEarningsHistoryVtuNationQueryHandler : IRequestHandler<GetEarningsHistoryVtuNationQuery, GetEarningsHistoryVtuNationResponse>
{
    private readonly IGetAdminServicesFromVtuNation _getAdminServicesFromVtuNation;
    private readonly ILogger<GetEarningsHistoryVtuNationQueryHandler> _logger;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;

    public GetEarningsHistoryVtuNationQueryHandler(IGetAdminServicesFromVtuNation getAdminServicesFromVtuNation, 
        ILogger<GetEarningsHistoryVtuNationQueryHandler> logger, 
        IUserContext userContext, IResourceBaseAuthorizationService resourceBaseAuthorizationService)
    {
        _getAdminServicesFromVtuNation = getAdminServicesFromVtuNation;
        _logger = logger;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
    }

    public async Task<GetEarningsHistoryVtuNationResponse> Handle(GetEarningsHistoryVtuNationQuery request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.GodsEyeOnly))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand?.Email ?? "Anonymous User",
                typeof(GetEarningsHistoryVtuNationQuery).Name,
                request);

            throw new ForbiddenAccessException();
        }

        var getEarningsHistoryVtuNationResponse = new GetEarningsHistoryVtuNationResponse
        {
            GetEarningsHistoryResponseVtuNation = new()
        };

        var response = await _getAdminServicesFromVtuNation.GetEarningsHistoryVtuNationAsync();

        if (response.IsSuccessful)
        {
            getEarningsHistoryVtuNationResponse.GetEarningsHistoryResponseVtuNation = response.Content;
            getEarningsHistoryVtuNationResponse.Success = true;
            getEarningsHistoryVtuNationResponse.Message = $"Successfully Sent GetEarningsHistory Request to VtuNationApi";
        }
        else
        {
            _logger.LogError("Unable to process {NameOfRequest} from External Api {Name} at {time}",
                nameof(GetEarningsHistoryVtuNationQuery),
                "VtuNationApi",
                DateTimeOffset.UtcNow
            );

            // if response is null, it returns an empty list or collection
            getEarningsHistoryVtuNationResponse.Success = false;
            getEarningsHistoryVtuNationResponse.Message = $"Error processing your request. Please try again later";
            getEarningsHistoryVtuNationResponse.GetEarningsHistoryResponseVtuNation = null;
        }

        return getEarningsHistoryVtuNationResponse;
    }
}
