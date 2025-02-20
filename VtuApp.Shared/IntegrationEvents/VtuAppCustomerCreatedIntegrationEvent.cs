namespace VtuApp.Shared.IntegrationEvents;

public record VtuAppCustomerCreatedIntegrationEvent(
    Guid ApplicationUserId,
    string Email);
