namespace VtuApp.Shared.IntegrationEvents;

public record NotifyUserOfStarAchievedEvent(
    Guid CustomerId,
    string Email,
    string FirstName,
    decimal DiscountGiven,
    decimal TotalOfTransactionsMade,
    decimal FinalVtuBonusBalance,
    DateTimeOffset CreatedAt);