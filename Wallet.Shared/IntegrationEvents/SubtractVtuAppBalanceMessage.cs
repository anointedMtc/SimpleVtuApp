namespace Wallet.Shared.IntegrationEvents;

public record SubtractVtuAppBalanceMessage(
    Guid WalletId,
    Guid OwnerId,
    Guid ApplicationUserId,
    string Email,
    //string FirstName,
    string ReasonWhy,
    Guid TransferID,
    decimal Amount,
    decimal FinalBalance,
    DateTimeOffset CreatedAt);