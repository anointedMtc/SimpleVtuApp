namespace VtuApp.Shared.IntegrationEvents;

public record VtuBonusTransferredToWalletEvent(
    Guid ApplicationUserId,
    string Email,
    string FirstName,
    decimal AmountTransferred,
    DateTimeOffset CreatedAt,
    Guid VtuBonusTransferId,
    decimal InitialBonusBalance,
    decimal FinalBonusBalance);
