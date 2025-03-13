using Identity.Shared.Constants;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Exceptions;
using SharedKernel.Application.Interfaces;
using VtuApp.Application.Interfaces.ExternalServices.VtuNationApi;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Account.Commands.SetUpdateTransactionPassVtuNation;

internal sealed class SetUpdateTransactionPassVtuNationCommandHandler : IRequestHandler<SetUpdateTransactionPassVtuNationCommand, SetUpdateTransactionPassVtuNationResponse>
{
    private readonly IGetAdminServicesFromVtuNation _getAdminServicesFromVtuNation;
    private readonly ILogger<SetUpdateTransactionPassVtuNationCommandHandler> _logger;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;

    public SetUpdateTransactionPassVtuNationCommandHandler(IGetAdminServicesFromVtuNation getAdminServicesFromVtuNation, 
        ILogger<SetUpdateTransactionPassVtuNationCommandHandler> logger, 
        IUserContext userContext, IResourceBaseAuthorizationService resourceBaseAuthorizationService)
    {
        _getAdminServicesFromVtuNation = getAdminServicesFromVtuNation;
        _logger = logger;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
    }

    public async Task<SetUpdateTransactionPassVtuNationResponse> Handle(SetUpdateTransactionPassVtuNationCommand request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.GodsEyeOnly))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand?.Email ?? "Anonymous User",
                typeof(SetUpdateTransactionPassVtuNationCommand).Name,
                request);

            throw new ForbiddenAccessException();
        }

        var setUpdateTransactionPassVtuNationResponse = new SetUpdateTransactionPassVtuNationResponse
        {
            SetUpdateTransactionPassResponseVtuNation = new()
        };

        var response = await _getAdminServicesFromVtuNation.SetUpdateTransactionPassVtuNationAsync(request.SetUpdateTransactionPassRequestVtuNation);

        if (response.IsSuccessful)
        {
            setUpdateTransactionPassVtuNationResponse.SetUpdateTransactionPassResponseVtuNation = response.Content;
            setUpdateTransactionPassVtuNationResponse.Success = true;
            setUpdateTransactionPassVtuNationResponse.Message = $"Successfully sent SetUpdateTransactionPass Request to VtuNationApi";
        }
        else
        {
            _logger.LogError("Unable to process {NameOfRequest} from External Api {Name} at {time} with error message {Error.Message}",
                nameof(SetUpdateTransactionPassVtuNationCommand),
                "VtuNationApi",
                DateTimeOffset.UtcNow,
                response.Error.Message
            );

            // if response is null, it returns an empty list or collection
            setUpdateTransactionPassVtuNationResponse.Success = false;
            setUpdateTransactionPassVtuNationResponse.Message = $"---{response.StatusCode}---{response.Error.Message}---{response.Error.InnerException}";
            setUpdateTransactionPassVtuNationResponse.SetUpdateTransactionPassResponseVtuNation = null;
        }

        return setUpdateTransactionPassVtuNationResponse;
    }
}
