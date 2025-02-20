using Refit;
using VtuApp.Shared.DTO.VtuNationApi.AdminServices.Account;
using VtuApp.Shared.DTO.VtuNationApi.AdminServices.Auth;
using VtuApp.Shared.DTO.VtuNationApi.AdminServices.Complaint;
using VtuApp.Shared.DTO.VtuNationApi.AdminServices.Earnings;
using VtuApp.Shared.DTO.VtuNationApi.AdminServices.Funding;
using VtuApp.Shared.DTO.VtuNationApi.AdminServices.Transaction;

namespace VtuApp.Application.Interfaces.ExternalServices.VtuNationApi;

public interface IGetAdminServicesFromVtuNation
{
    // Auth

    [Post("/api/auth/register")]
    Task<ApiResponse<RegisterResponseVtuNation>> RegisterWithVtuNationApiAsync([Body] RegisterRequestVtuNation registerRequestVtuNation);

    [Post("/api/auth/forgotPassword")]
    Task<ApiResponse<ForgotPasswordResponseVtuNation>> ForgotPasswordVtuNationAsync([Body] ForgotPasswordRequestVtuNation forgotPasswordRequestVtuNation);

    [Get("/api/auth/user-profile")]
    Task<ApiResponse<GetProfileResponseVtuNation>> GetProfileVtuNationAsync();

    [Post("/api/auth/refresh")]
    Task<ApiResponse<RefreshTokenResponseVtuNation>> RefreshTokenVtuNationAsync();

    [Post("api/auth/logout")]
    Task<ApiResponse<LogOutResponseVtuNation>> LogOutVtuNationAsync();



    // Account

    [Post("/api/account/update_transaction_pass")]
    Task<ApiResponse<SetUpdateTransactionPassResponseVtuNation>> SetUpdateTransactionPassVtuNationAsync([Body] SetUpdateTransactionPassRequestVtuNation setUpdateTransactionPassRequestVtuNation);

    [Post("/api/account/update_password")]
    Task<ApiResponse<UpdatePasswordResponseVtuNation>> UpdatePasswordVtuNationAsync([Body] UpdatePasswordRequestVtuNation updatePasswordRequestVtuNation);

    [Post("/api/account/update_profile")]
    Task<ApiResponse<UpdateProfileResponseVtuNation>> UpdateProfileVtuNationAsync([Body] UpdateProfileRequestVtuNation updateProfileRequestVtuNation);

    [Post("/api/account/send_verification_code")]
    Task<ApiResponse<SendEmailVerificationLinkResponseVtuNation>> SendEmailVerificationLinkVtuNationAsync();

    [Post("/api/account/add_bvn")]
    Task<ApiResponse<SubmitBvnResponseVtuNation>> SubmitBvnVtuNationAsync([Body] SubmitBvnRequestVtuNation submitBvnRequestVtuNation);

    [Post("/api/account/confirm_email")]
    Task<ApiResponse<ConfirmEmailResponseVtuNation>> ConfirmEmailVtuNationAsync([Body] ConfirmEmailRequestVtuNation confirmEmailRequestVtuNation);

    [Post("/api/account/generate_otp")]
    Task<ApiResponse<GenerateOtpResponseVtuNation>> GenerateOtpVtuNationAsync([Body] GenerateOtpRequestVtuNation generateOtpRequestVtuNation);

    [Post("/api/account/validate_otp")]
    Task<ApiResponse<ValidateOtpResponseVtuNation>> ValidateOtpVtuNationAsync([Body] ValidateOtpRequestVtuNation validateOtpRequestVtuNation);



    // Transactions

    [Get("/api/get_transactions")]
    Task<ApiResponse<GetTransactionHistoryResponseVtuNation>> GetTransactionHistoryVtuNationAsync();

    [Get("/api/get_transaction/{id}")]
    Task<ApiResponse<GetSingleTransactionResponseVtuNation>> GetSingleTransactionVtuNationAsync(string id);



    // Complaint
    [Get("/api/add_complaint")]
    Task<ApiResponse<AddComplaintResponseVtuNation>> AddComplaintVtuNationAsync([Body] AddComplaintRequestVtuNation addComplaintRequestVtuNation);


    // Funding
    [Get("/api/funding/payment_account_info")]
    Task<ApiResponse<PaymentAccountInfoResponseVtuNation>> GetPaymentAccountInfoVtuNationAsync();

    [Post("/api/funding/send_fund_notification")]
    Task<ApiResponse<SubmitPaymentNotificationResponseVtuNation>> SubmitPaymentNotificationVtuNationAsync([Body] SubmitPaymentNotificationRequestVtuNation submitPaymentNotificationRequestVtuNation);

    [Get("/api/funding/get_fund_notifications")]
    Task<ApiResponse<GetFundNotificationsResponseVtuNation>> GetFundNotificationsVtuNationAsync();



    // Earnings
    [Post("/api/earnings/transfer_to_main_wallet")]
    Task<ApiResponse<TransferBonusToMainWalletResponseVtuNation>> TransferBonusToMainWalletVtuNationAsync([Body] TransferBonusToMainWalletRequestVtuNation transferBonusToMainWalletRequestVtuNation);

    [Get("/api/earnings/history")]
    Task<ApiResponse<GetEarningsHistoryResponseVtuNation>> GetEarningsHistoryVtuNationAsync();

}
