namespace VtuApp.Shared.IntegrationEvents;

public record CreateNewVtuAppCustomerMessage(
    Guid ApplicationUserId,
    string UserEmail,
    string UserFirstName,
    string UserLastName,
    string PhoneNumber,
    decimal RegisterationBonus);