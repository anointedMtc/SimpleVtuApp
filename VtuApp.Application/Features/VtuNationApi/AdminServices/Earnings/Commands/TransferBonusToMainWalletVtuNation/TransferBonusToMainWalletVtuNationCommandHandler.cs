using ApplicationSharedKernel.Exceptions;
using ApplicationSharedKernel.Interfaces;
using Identity.Shared.Constants;
using MediatR;
using Microsoft.Extensions.Logging;
using VtuApp.Application.Interfaces.ExternalServices.VtuNationApi;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Earnings.Commands.TransferBonusToMainWalletVtuNation;

internal sealed class TransferBonusToMainWalletVtuNationCommandHandler : IRequestHandler<TransferBonusToMainWalletVtuNationCommand, TransferBonusToMainWalletVtuNationResponse>
{
    private readonly IGetAdminServicesFromVtuNation _getAdminServicesFromVtuNation;
    private readonly ILogger<TransferBonusToMainWalletVtuNationCommandHandler> _logger;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;

    public TransferBonusToMainWalletVtuNationCommandHandler(IGetAdminServicesFromVtuNation getAdminServicesFromVtuNation, 
        ILogger<TransferBonusToMainWalletVtuNationCommandHandler> logger, 
        IUserContext userContext, IResourceBaseAuthorizationService resourceBaseAuthorizationService)
    {
        _getAdminServicesFromVtuNation = getAdminServicesFromVtuNation;
        _logger = logger;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
    }

    public async Task<TransferBonusToMainWalletVtuNationResponse> Handle(TransferBonusToMainWalletVtuNationCommand request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.GodsEyeOnly))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand?.Email ?? "Anonymous User",
                typeof(TransferBonusToMainWalletVtuNationCommand).Name,
                request);

            throw new ForbiddenAccessException();
        }

        var transferBonusToMainWalletVtuNationResponse = new TransferBonusToMainWalletVtuNationResponse
        {
            TransferBonusToMainWalletResponseVtuNation = new()
        };

        var response = await _getAdminServicesFromVtuNation.TransferBonusToMainWalletVtuNationAsync(request.TransferBonusToMainWalletRequestVtuNation);

        if (response.IsSuccessful)
        {
            transferBonusToMainWalletVtuNationResponse.TransferBonusToMainWalletResponseVtuNation = response.Content;
            transferBonusToMainWalletVtuNationResponse.Success = true;
            transferBonusToMainWalletVtuNationResponse.Message = $"Successfully Sent TransferBonusToMainWallet Request to VtuNationApi";
        }
        else
        {
            _logger.LogError("Unable to process {NameOfRequest} from External Api {Name} at {time}",
                nameof(TransferBonusToMainWalletVtuNationCommand),
                "VtuNationApi",
                DateTimeOffset.UtcNow
            );

            // if response is null, it returns an empty list or collection
            transferBonusToMainWalletVtuNationResponse.Success = false;
            transferBonusToMainWalletVtuNationResponse.Message = $"Error processing your request. Please try again later";
            transferBonusToMainWalletVtuNationResponse.TransferBonusToMainWalletResponseVtuNation = null;
        }

        return transferBonusToMainWalletVtuNationResponse;
    }
}
