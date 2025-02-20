using ApplicationSharedKernel.Exceptions;
using ApplicationSharedKernel.Interfaces;
using Identity.Shared.Constants;
using MediatR;
using Microsoft.Extensions.Logging;
using VtuApp.Application.Interfaces.ExternalServices.VtuNationApi;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Account.Commands.GenerateOtpVtuNation;

internal sealed class GenerateOtpVtuNationCommandHandler : IRequestHandler<GenerateOtpVtuNationCommand, GenerateOtpVtuNationResponse>
{
    private readonly IGetAdminServicesFromVtuNation _getAdminServicesFromVtuNation;
    private readonly ILogger<GenerateOtpVtuNationCommandHandler> _logger;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;

    public GenerateOtpVtuNationCommandHandler(IGetAdminServicesFromVtuNation getAdminServicesFromVtuNation, 
        ILogger<GenerateOtpVtuNationCommandHandler> logger, 
        IUserContext userContext, IResourceBaseAuthorizationService resourceBaseAuthorizationService)
    {
        _getAdminServicesFromVtuNation = getAdminServicesFromVtuNation;
        _logger = logger;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
    }

    public async Task<GenerateOtpVtuNationResponse> Handle(GenerateOtpVtuNationCommand request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.GodsEyeOnly))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand?.Email ?? "Anonymous User",
                typeof(GenerateOtpVtuNationCommand).Name,
                request);

            throw new ForbiddenAccessException();
        }

        var generateOtpVtuNationResponse = new GenerateOtpVtuNationResponse
        {
            GenerateOtpResponseVtuNation = new()
        };

        var response = await _getAdminServicesFromVtuNation.GenerateOtpVtuNationAsync(request.GenerateOtpRequestVtuNation);

        if (response.IsSuccessful)
        {
            generateOtpVtuNationResponse.GenerateOtpResponseVtuNation = response.Content;
            generateOtpVtuNationResponse.Success = true;
            generateOtpVtuNationResponse.Message = $"Successfully sent GenerateOtp Request to VtuNationApi";
        }
        else
        {
            _logger.LogError("Unable to process {NameOfRequest} from External Api {Name} at {time}",
                nameof(GenerateOtpVtuNationCommand),
                "VtuNationApi",
                DateTimeOffset.UtcNow
            );

            // if response is null, it returns an empty list or collection
            generateOtpVtuNationResponse.Success = false;
            generateOtpVtuNationResponse.Message = $"Error processing your request. Please try again later";
            generateOtpVtuNationResponse.GenerateOtpResponseVtuNation = null;
        }

        return generateOtpVtuNationResponse;
    }
}
