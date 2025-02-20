using VtuApp.Shared.Constants;

namespace VtuApp.Shared.IntegrationEvents;

public record BuyDataForCustomerSuccessEvent(
    Guid ApplicationUserId,
    string Email,
    Guid VtuTransactionId,
    NetworkProvider NetworkProvider,
    string DataPlanPurchased,
    decimal AmountPurchased,
    string Reciever);