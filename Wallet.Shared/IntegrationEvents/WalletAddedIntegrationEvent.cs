namespace Wallet.Shared.IntegrationEvents;

public record WalletAddedIntegrationEvent(
    Guid ApplicationUserId,
    string Email);
