using ApplicationSharedKernel.Interfaces;
using Identity.Domain.Entities;
using Identity.Shared.Constants;
using Identity.Shared.IntegrationEvents;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Identity.Application.Features.UsersEndpoints.EmailConfirmation;

public class EmailConfirmationCommandHandler : IRequestHandler<EmailConfirmationCommand, EmailConfirmationResponse>
{
    private readonly ILogger _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMassTransitService _massTransitService;

    public EmailConfirmationCommandHandler(ILogger<EmailConfirmationCommandHandler> logger, 
        UserManager<ApplicationUser> userManager, IMassTransitService massTransitService)
    {
        _logger = logger;
        _userManager = userManager;
        _massTransitService = massTransitService;
    }

    public async Task<EmailConfirmationResponse> Handle(EmailConfirmationCommand request, CancellationToken cancellationToken)
    {
        var emailConfirmationResponse = new EmailConfirmationResponse();

        var user = await _userManager.FindByEmailAsync(request.EmailConfirmation.Email);  // try to fetch the user using the email the client app sent in the request
        if (user is null)
            throw new Exception("Invalid Email Confirmation Request");

        var originalToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.EmailConfirmation.Token!));

        var confirmResult = await _userManager.ConfirmEmailAsync(user, originalToken);  // it validates if the email confirmation logic token matches the specified user
        if (!confirmResult.Succeeded)
            throw new Exception("Invalid Email Confirmation Request");

        emailConfirmationResponse.Success = true;
        emailConfirmationResponse.Message = $"Email confirmed successfully, you can proceed to login";

        var timeOfConfirmation = DateTimeOffset.UtcNow;

        user.UpdatedAt = timeOfConfirmation;
        _logger.LogInformation("{User} confirmed their email at {Time}", user.Email, timeOfConfirmation);

        await _massTransitService.Publish(new ApplicationUserEmailConfirmedEvent(
            new Guid(user.Id), 
            user.FirstName, 
            user.LastName, 
            user.Email!, 
            user.PhoneNumber!, 
            RegisterationBonusBalance.DefaultBonus)
        );

        _logger.LogInformation("This command {typeOfPublisher} Successfully published integration event {typeOfEvent} for applicationUser with Id {applicationUserId} at {time}",
           nameof(EmailConfirmationCommandHandler),
           nameof(ApplicationUserEmailConfirmedEvent),
           user.Email,
           DateTimeOffset.UtcNow
        );

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            _logger.LogError("Failed to update user: {errors}", errors);
            throw new Exception($"Failed to update user: {errors}");
        }

        return emailConfirmationResponse;
    }
}
