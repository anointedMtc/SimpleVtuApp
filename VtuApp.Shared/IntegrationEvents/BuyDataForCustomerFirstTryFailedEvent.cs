using VtuApp.Shared.Constants;

namespace VtuApp.Shared.IntegrationEvents;

public record BuyDataForCustomerFirstTryFailedEvent(
    Guid ApplicationUserId,
    string Email,
    Guid VtuTransactionId,
    NetworkProvider NetworkProvider,
    string DataPlanPurchased,
    decimal AmountToPurchase,
    string Reciever);
