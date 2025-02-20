using VtuApp.Shared.Constants;

namespace VtuApp.Shared.IntegrationEvents;

public record BuyAirtimeForCustomerSuccessEvent(
    Guid ApplicationUserId,
    string Email,
    Guid VtuTransactionId,
    NetworkProvider NetworkProvider,
    decimal AmountPurchased,
    string Reciever);