namespace SharedKernel.Application.Constants;

public static class QueueConstants
{
    // New Application user Created
    public const string CreateNewWalletOwnerMessageQueueName = "create-new-wallet-owner-message";

    public const string CreateNewVtuAppCustomerMessageQueueName = "create-new-vtu-app-customer-message";


    // Vtu-Airtime
    public const string BuyAirtimeForCustomerMessageQueueName = "buy-airtime-for-customer-message";

    public const string NotifyCustomerOfVtuAirtimePurchaseSuccessEventQueueName = "notify-customer-of-vtu-airtime-purchase-success";

    public const string DeductFundsFromCustomerWalletForVtuPurchaseTransactionMessageQueueName = "deduct-funds-from-customer-wallet-for-vtu-purchase-transaction-message";

    public const string NotifyCustomerOfVtuAirtimePurchaseFailedEventQueueName = "notify-customer-of-vtu-airtime-purchase-failed";

    public const string RollbackAmountForVtuAirtimePurchaseFailedMessageQueueName = "rollback-amount-for-vtu-airtime-purchase-failed-message";


    // Vtu-Data
    public const string BuyDataForCustomerMessageQueueName = "buy-data-for-customer-message";

    public const string NotifyCustomerOfVtuDataPurchaseFailedEventQueueName = "notify-customer-of-vtu-data-purchase-failed";

    public const string RollbackAmountForVtuDataPurchaseFailedMessageQueueName = "rollback-amount-for-vtu-data-purchase-failed-message";

    public const string NotifyCustomerOfVtuDataPurchaseSuccessEventQueueName = "notify-customer-of-vtu-data-purchase-success";


}
