using Identity.Shared.Constants;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Exceptions;
using SharedKernel.Application.Interfaces;
using VtuApp.Application.Interfaces.ExternalServices.VtuNationApi;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Account.Commands.SubmitBvnVtuNation;

internal sealed class SubmitBvnVtuNationCommandHandler : IRequestHandler<SubmitBvnVtuNationCommand, SubmitBvnVtuNationResponse>
{
    private readonly IGetAdminServicesFromVtuNation _getAdminServicesFromVtuNation;
    private readonly ILogger<SubmitBvnVtuNationCommandHandler> _logger;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;

    public SubmitBvnVtuNationCommandHandler(IGetAdminServicesFromVtuNation getAdminServicesFromVtuNation, 
        ILogger<SubmitBvnVtuNationCommandHandler> logger, IUserContext userContext, 
        IResourceBaseAuthorizationService resourceBaseAuthorizationService)
    {
        _getAdminServicesFromVtuNation = getAdminServicesFromVtuNation;
        _logger = logger;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
    }

    public async Task<SubmitBvnVtuNationResponse> Handle(SubmitBvnVtuNationCommand request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.GodsEyeOnly))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand?.Email ?? "Anonymous User",
                typeof(SubmitBvnVtuNationCommand).Name,
                request);

            throw new ForbiddenAccessException();
        }

        var SubmitBvnVtuNationResponse = new SubmitBvnVtuNationResponse
        {
            SubmitBvnResponseVtuNation = new()
        };

        var response = await _getAdminServicesFromVtuNation.SubmitBvnVtuNationAsync(request.SubmitBvnRequestVtuNation);

        if (response.IsSuccessful)
        {
            SubmitBvnVtuNationResponse.SubmitBvnResponseVtuNation = response.Content;
            SubmitBvnVtuNationResponse.Success = true;
            SubmitBvnVtuNationResponse.Message = $"Successfully Submited Bvn to VtuNationApi";
        }
        else
        {
            _logger.LogError("Unable to process {NameOfRequest} from External Api {Name} at {time} with error message {Error.Message}",
                nameof(SubmitBvnVtuNationCommand),
                "VtuNationApi",
                DateTimeOffset.UtcNow,
                response.Error.Message
            );

            // if response is null, it returns an empty list or collection
            SubmitBvnVtuNationResponse.Success = false;
            SubmitBvnVtuNationResponse.Message = $"---{response.StatusCode}---{response.Error.Message}---{response.Error.InnerException}";
            SubmitBvnVtuNationResponse.SubmitBvnResponseVtuNation = null;
        }

        return SubmitBvnVtuNationResponse;
    }
}
