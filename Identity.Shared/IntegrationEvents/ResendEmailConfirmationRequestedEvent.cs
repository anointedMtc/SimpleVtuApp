namespace Identity.Shared.IntegrationEvents;

public record ResendEmailConfirmationRequestedEvent(
    string Email,
    string FirstName,
    string CallbackUrl,
    string ValidToken);
