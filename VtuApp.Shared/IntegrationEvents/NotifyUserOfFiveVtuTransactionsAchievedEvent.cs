namespace VtuApp.Shared.IntegrationEvents;

public record NotifyUserOfFiveVtuTransactionsAchievedEvent(
    Guid CustomerId,
    string Email,
    string FirstName,
    decimal BonusForFiveTransactions,
    decimal FinalVtuBonusBalance,
    DateTimeOffset CreatedAt);