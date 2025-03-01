namespace VtuApp.Shared.IntegrationEvents;

public record NotifyUserOfFundsAddedEvent(
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