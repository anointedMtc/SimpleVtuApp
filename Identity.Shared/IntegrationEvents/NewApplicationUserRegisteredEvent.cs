namespace Identity.Shared.IntegrationEvents;

// I want this event to be consumed ONLY BY THE NOTIFICATION MODULE
public record NewApplicationUserRegisteredEvent(
    string Email,
    string FirstName,
    string CallbackUrl, 
    string ValidToken);

