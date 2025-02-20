using ApplicationSharedKernel.Interfaces;
using Identity.Application.Exceptions;
using Identity.Domain.Entities;
using Identity.Shared.Constants;
using Identity.Shared.IntegrationEvents;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Identity.Application.Features.Utilities.TriggerUserCreatedEventSaga;

internal sealed class TriggerUserCreatedEventSagaCommandHandler : IRequestHandler<TriggerUserCreatedEventSagaCommand, TriggerUserCreatedEventSagaResponse>
{
    private readonly ILogger<TriggerUserCreatedEventSagaCommandHandler> _logger;
    private readonly IMassTransitService _massTransitService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;
    private readonly IUserContext _userContext;

    public TriggerUserCreatedEventSagaCommandHandler(
        ILogger<TriggerUserCreatedEventSagaCommandHandler> logger, 
        IMassTransitService massTransitService, 
        UserManager<ApplicationUser> userManager, 
        IResourceBaseAuthorizationService resourceBaseAuthorizationService, 
        IUserContext userContext)
    {
        _logger = logger;
        _massTransitService = massTransitService;
        _userManager = userManager;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
        _userContext = userContext;
    }

    public async Task<TriggerUserCreatedEventSagaResponse> Handle(TriggerUserCreatedEventSagaCommand request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.GodsEyeOnly))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand!.Email ?? "AnonymousUser",
                typeof(TriggerUserCreatedEventSagaCommand),
                request
            );

            throw new CustomForbiddenException("Access Denied. You do not have Permission to view this resource");
        }

        var triggerUserCreatedEventSagaResponse = new TriggerUserCreatedEventSagaResponse();

        var user = await _userManager.FindByEmailAsync(request.UserEmail);
        if (user == null)
        {
            _logger.LogError("Admin with id {adminId} tried to peform {typeOfOperation} for a user with Id {userEmail} that does not exitst at {time}",
                userExecutingCommand!.Email,
                nameof(TriggerUserCreatedEventSagaCommand),
                request.UserEmail,
                DateTimeOffset.UtcNow
            );

            //throw new Exception("User not found");
            triggerUserCreatedEventSagaResponse.Success = false;
            triggerUserCreatedEventSagaResponse.Message = $"Bad Request";

            return triggerUserCreatedEventSagaResponse;
        }

        await _massTransitService.Publish(new ApplicationUserEmailConfirmedEvent(
            new Guid(user!.Id),
            user.FirstName,
            user.LastName,
            user.Email!,
            user.PhoneNumber!,
            RegisterationBonusBalance.DefaultBonus)
        );

        _logger.LogInformation("This command {typeOfPublisher} Successfully published integration event {typeOfEvent} for applicationUser with Id {applicationUserId} at {time}",
           nameof(TriggerUserCreatedEventSagaCommand),
           nameof(ApplicationUserEmailConfirmedEvent),
           user.Email,
           DateTimeOffset.UtcNow
        );

        triggerUserCreatedEventSagaResponse.Success = true;
        triggerUserCreatedEventSagaResponse.Message = $"User Created Event Saga was successfully triggered for user with Id {request.UserEmail}";

        return triggerUserCreatedEventSagaResponse;
    }
}
