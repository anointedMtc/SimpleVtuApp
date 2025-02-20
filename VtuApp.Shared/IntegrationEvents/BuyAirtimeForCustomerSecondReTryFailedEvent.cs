using VtuApp.Shared.Constants;

namespace VtuApp.Shared.IntegrationEvents;

public record BuyAirtimeForCustomerSecondReTryFailedEvent(
    Guid ApplicationUserId,
    string Email,
    Guid VtuTransactionId,
    NetworkProvider NetworkProvider,
    decimal AmountToPurchase,
    string Reciever);