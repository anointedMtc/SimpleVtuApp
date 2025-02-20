namespace Identity.Shared.IntegrationEvents;

public record TwoFacAuthRequestedEvent(
    string Email,
    string TokenFor2Fac,
    string FirstName);

