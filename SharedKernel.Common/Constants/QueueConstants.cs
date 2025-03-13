namespace SharedKernel.Common.Constants;

public static class QueueConstants
{
    // New Application user Created
    public const string CreateNewWalletOwnerMessageQueueName = "create-new-wallet-owner-message";

    public const string CreateNewVtuAppCustomerMessageQueueName = "create-new-vtu-app-customer-message";


    // Vtu-Airtime
    public const string BuyAirtimeForCustomerMessageQueueName = "buy-airtime-for-customer-message";

    public const string NotifyCustomerOfVtuAirtimePurchaseSuccessEventQueueName = "notify-customer-of-vtu-airtime-purchase-success-event";

    public const string DeductFundsFromCustomerWalletForVtuPurchaseTransactionMessageQueueName = "deduct-funds-from-customer-wallet-for-vtu-purchase-transaction-message";

    public const string NotifyCustomerOfVtuAirtimePurchaseFailedEventQueueName = "notify-customer-of-vtu-airtime-purchase-failed-event";

    public const string RollbackAmountForVtuAirtimePurchaseFailedMessageQueueName = "rollback-amount-for-vtu-airtime-purchase-failed-message";


    // Vtu-Data
    public const string BuyDataForCustomerMessageQueueName = "buy-data-for-customer-message";

    public const string NotifyCustomerOfVtuDataPurchaseFailedEventQueueName = "notify-customer-of-vtu-data-purchase-failed-event";

    public const string RollbackAmountForVtuDataPurchaseFailedMessageQueueName = "rollback-amount-for-vtu-data-purchase-failed-message";

    public const string NotifyCustomerOfVtuDataPurchaseSuccessEventQueueName = "notify-customer-of-vtu-data-purchase-success-event";




    // Wallet
    public const string AddVtuAppBalanceMessageQueueName = "add-vtu-app-balance-message";
   
    public const string SubtractVtuAppBalanceMessageQueueName = "subtract-vtu-app-balance-message";

}
