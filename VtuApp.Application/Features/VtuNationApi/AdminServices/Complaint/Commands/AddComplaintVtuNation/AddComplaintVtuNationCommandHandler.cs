using Identity.Shared.Constants;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Exceptions;
using SharedKernel.Application.Interfaces;
using VtuApp.Application.Interfaces.ExternalServices.VtuNationApi;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Complaint.Commands.AddComplaintVtuNation;

internal sealed class AddComplaintVtuNationCommandHandler : IRequestHandler<AddComplaintVtuNationCommand, AddComplaintVtuNationResponse>
{
    private readonly IGetAdminServicesFromVtuNation _getAdminServicesFromVtuNation;
    private readonly ILogger<AddComplaintVtuNationCommandHandler> _logger;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;

    public AddComplaintVtuNationCommandHandler(IGetAdminServicesFromVtuNation getAdminServicesFromVtuNation, 
        ILogger<AddComplaintVtuNationCommandHandler> logger, IUserContext userContext, 
        IResourceBaseAuthorizationService resourceBaseAuthorizationService)
    {
        _getAdminServicesFromVtuNation = getAdminServicesFromVtuNation;
        _logger = logger;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
    }

    public async Task<AddComplaintVtuNationResponse> Handle(AddComplaintVtuNationCommand request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.GodsEyeOnly))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand?.Email ?? "Anonymous User",
                typeof(AddComplaintVtuNationCommand).Name,
                request);

            throw new ForbiddenAccessException();
        }

        var addComplaintVtuNationResponse = new AddComplaintVtuNationResponse
        {
            AddComplaintResponseVtuNation = new()
        };

        var response = await _getAdminServicesFromVtuNation.AddComplaintVtuNationAsync(request.AddComplaintRequestVtuNation);

        if (response.IsSuccessful)
        {
            addComplaintVtuNationResponse.AddComplaintResponseVtuNation = response.Content;
            addComplaintVtuNationResponse.Success = true;
            addComplaintVtuNationResponse.Message = $"Successfully Sent GetProfile Request to VtuNationApi";
        }
        else
        {
            _logger.LogError("Unable to process {NameOfRequest} from External Api {Name} at {time}",
                nameof(AddComplaintVtuNationCommand),
                "VtuNationApi",
                DateTimeOffset.UtcNow
            );

            // if response is null, it returns an empty list or collection
            addComplaintVtuNationResponse.Success = false;
            addComplaintVtuNationResponse.Message = $"Error processing your request. Please try again later";
            addComplaintVtuNationResponse.AddComplaintResponseVtuNation = null;
        }

        return addComplaintVtuNationResponse;
    }
}
