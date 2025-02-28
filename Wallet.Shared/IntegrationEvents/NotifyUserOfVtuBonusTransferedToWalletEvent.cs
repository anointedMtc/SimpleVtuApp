namespace Wallet.Shared.IntegrationEvents;

public record NotifyUserOfVtuBonusTransferedToWalletEvent(
    Guid ApplicationUserId,
    string Email,
    string FirstName,
    Guid VtuBonusTransferId,
    decimal AmountTransfered,
    decimal InitialBalance,
    decimal FinalBalance,
    DateTimeOffset CreatedAt);