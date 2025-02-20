namespace Identity.Shared.IntegrationEvents;

public record NotifyUserOfAccountLockOutEvent(
    string Email,
    string FirstName);
