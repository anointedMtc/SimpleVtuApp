namespace Identity.Shared.IntegrationEvents;

public record ChangeEmailRequestedEvent(
    string OldEmail,
    string NewEmail,
    string CallBackUrl,
    string Token,
    string FirstName);

