namespace Wallet.Shared.IntegrationEvents;

public record NotifyOwnersOfFundsTransferredEvent(
    Guid FromWalletId,
    Guid FromWalletTransferId,
    string FromWalletFirstName,
    string FromWalletEmail,
    decimal FromWalletBalance,

    Guid ToWalletId,
    Guid ToWalletTransferId,
    string ToWalletFirstName,
    string ToWalletEmail,
    decimal ToWalletBalance,

    decimal Amount,
    DateTimeOffset CreatedAt);
