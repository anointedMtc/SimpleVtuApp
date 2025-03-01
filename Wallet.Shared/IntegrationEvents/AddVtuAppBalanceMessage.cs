namespace Wallet.Shared.IntegrationEvents;

public record AddVtuAppBalanceMessage(
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
