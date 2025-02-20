using VtuApp.Shared.Constants;

namespace VtuApp.Shared.IntegrationEvents;

public record FundsRefundedForVtuPurchaseFailureEvent(
    Guid ApplicationUserId,
    string Email,
    string FirstName,
    Guid VtuTransactionId,
    NetworkProvider NetworkProvider,
    TypeOfTransaction TypeOfTransaction,
    string? DataValue,
    decimal AmountPurchased,
    decimal PricePaid,
    string Receiver,
    DateTimeOffset CreatedAt);

