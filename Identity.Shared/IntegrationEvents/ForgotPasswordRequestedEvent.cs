namespace Identity.Shared.IntegrationEvents;

public record ForgotPasswordRequestedEvent(
    string Email,
    string CallBackUrl,
    string Token,
    string FirstName);