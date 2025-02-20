namespace Wallet.Shared.IntegrationEvents;

public record CreateNewWalletOwnerMessage(
    Guid ApplicationUserId, 
    string UserEmail, 
    string UserFirstName, 
    string UserLastName,
    decimal RegisterationBonus);
