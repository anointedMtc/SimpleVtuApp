namespace VtuApp.Shared.IntegrationEvents;

public record NotifyUserOfFundsDeductedEvent(
    Guid WalletId,
    Guid OwnerId,
    Guid ApplicationUserId,
    string Email,
    string FirstName,
    string ReasonWhy,
    Guid TransferId,
    decimal Amount,
    decimal FinalBalance,
    DateTimeOffset CreatedAt);