namespace VtuApp.Api.Controllers.V1;

[Authorize]
[ApiVersion("1.0")]
public class AdminServicesVtuNationController : ApiBaseController
{
    // ACCOUNT
    [HttpPost("confirm-email-vtuNation")]
    public async Task<ActionResult<ConfirmEmailVtuNationResponse>> ConfirmEmailVtuNation([FromBody] ConfirmEmailRequestVtuNation confirmEmailRequestVtuNation)
    {
        var result = await Mediator.Send(new ConfirmEmailVtuNationCommand() { ConfirmEmailRequestVtuNation = confirmEmailRequestVtuNation });

        return Ok(result);
    }


    [HttpPost("generate-otp-vtuNation")]
    public async Task<ActionResult<GenerateOtpVtuNationResponse>> GenerateOtpVtuNation([FromBody] GenerateOtpRequestVtuNation generateOtpRequestVtuNation)
    {
        var result = await Mediator.Send(new GenerateOtpVtuNationCommand() { GenerateOtpRequestVtuNation = generateOtpRequestVtuNation });

        return Ok(result);
    }


    [HttpPost("send-email-verification-link-vtuNation")]
    public async Task<ActionResult<SendEmailVerificationLinkVtuNationResponse>> SendEmailVerificationLinkVtuNation()
    {
        var result = await Mediator.Send(new SendEmailVerificationLinkVtuNationCommand());

        return Ok(result);
    }


    [HttpPost("set-update-transaction-pass-vtuNation")]
    public async Task<ActionResult<SetUpdateTransactionPassVtuNationResponse>> SetUpdateTransactionPassVtuNation([FromBody] SetUpdateTransactionPassRequestVtuNation setUpdateTransactionPassRequestVtuNation)
    {
        var result = await Mediator.Send(new SetUpdateTransactionPassVtuNationCommand() { SetUpdateTransactionPassRequestVtuNation = setUpdateTransactionPassRequestVtuNation });

        return Ok(result);
    }


    [HttpPost("submit-bvn-vtuNation")]
    public async Task<ActionResult<SubmitBvnVtuNationCommand>> SubmitBvnVtuNation([FromBody] SubmitBvnRequestVtuNation submitBvnRequestVtuNation)
    {
        var result = await Mediator.Send(new SubmitBvnVtuNationCommand() { SubmitBvnRequestVtuNation = submitBvnRequestVtuNation });

        return Ok(result);
    }


    [HttpPost("update-password-vtuNation")]
    public async Task<ActionResult<UpdatePasswordVtuNationResponse>> UpdatePasswordVtuNation([FromBody] UpdatePasswordRequestVtuNation updatePasswordRequestVtuNation)
    {
        var result = await Mediator.Send(new UpdatePasswordVtuNationCommand() { UpdatePasswordRequestVtuNation = updatePasswordRequestVtuNation });

        return Ok(result);
    }


    [HttpPost("update-profile-vtuNation")]
    public async Task<ActionResult<UpdateProfileVtuNationResponse>> UpdateProfileVtuNation([FromBody] UpdateProfileRequestVtuNation updateProfileRequestVtuNation)
    {
        var result = await Mediator.Send(new UpdateProfileVtuNationCommand() { UpdateProfileRequestVtuNation = updateProfileRequestVtuNation });

        return Ok(result);
    }


    [HttpPost("validate-otp-vtuNation")]
    public async Task<ActionResult<ValidateOtpVtuNationResponse>> ValidateOtpVtuNation([FromBody] ValidateOtpRequestVtuNation validateOtpRequestVtuNation)
    {
        var result = await Mediator.Send(new ValidateOtpVtuNationCommand() { ValidateOtpRequestVtuNation = validateOtpRequestVtuNation });

        return Ok(result);
    }


    // AUTH
    [HttpPost("forgot-password-vtuNation")]
    public async Task<ActionResult<ForgotPasswordVtuNationResponse>> ForgotPasswordVtuNation([FromBody] ForgotPasswordRequestVtuNation forgotPasswordRequestVtuNation)
    {
        var result = await Mediator.Send(new ForgotPasswordVtuNationCommand() { ForgotPasswordRequestVtuNation = forgotPasswordRequestVtuNation });

        return Ok(result);
    }


    [HttpPost("logout-vtuNation")]
    public async Task<ActionResult<LogOutVtuNationResponse>> LogoutVtuNation()
    {
        var result = await Mediator.Send(new LogOutVtuNationCommand());

        return Ok(result);
    }


    [HttpPost("refresh-token-vtuNation")]
    public async Task<ActionResult<RefreshTokenVtuNationResponse>> RefreshTokenVtuNation()
    {
        var result = await Mediator.Send(new RefreshTokenVtuNationCommand());

        return Ok(result);
    }


    [HttpPost("register-with-vtuNation")]
    public async Task<ActionResult<RegisterWithVtuNationApiResponse>> RegisterWithVtuNation([FromBody] RegisterRequestVtuNation registerRequestVtuNation)
    {
        var result = await Mediator.Send(new RegisterWithVtuNationApiCommand() { RegisterRequestVtuNation = registerRequestVtuNation });

        return Ok(result);
    }


    [HttpGet("get-profile-vtuNation")]
    public async Task<ActionResult<GetProfileVtuNationResponse>> GetProfileVtuNation()
    {
        var result = await Mediator.Send(new GetProfileVtuNationQuery());

        return Ok(result);
    }


    // COMPLAINT
    [HttpPost("add-complaint-vtuNation")]
    public async Task<ActionResult<AddComplaintVtuNationResponse>> AddComplaintVtuNation([FromBody] AddComplaintRequestVtuNation addComplaintRequestVtuNation)
    {
        var result = await Mediator.Send(new AddComplaintVtuNationCommand() { AddComplaintRequestVtuNation = addComplaintRequestVtuNation });

        return Ok(result);
    }


    // EARNINGS
    [HttpPost("transfer-bonus-to-main-wallet-vtuNation")]
    public async Task<ActionResult<TransferBonusToMainWalletVtuNationResponse>> TransferBonusToMainWalletVtuNation([FromBody] TransferBonusToMainWalletRequestVtuNation transferBonusToMainWalletRequestVtuNation)
    {
        var result = await Mediator.Send(new TransferBonusToMainWalletVtuNationCommand() { TransferBonusToMainWalletRequestVtuNation = transferBonusToMainWalletRequestVtuNation });

        return Ok(result);
    }


    [HttpGet("get-earnings-history-vtuNation")]
    public async Task<ActionResult<GetEarningsHistoryVtuNationResponse>> GetEarningsHistoryVtuNation()
    {
        var result = await Mediator.Send(new GetEarningsHistoryVtuNationQuery());

        return Ok(result);
    }


    // FUNDING
    [HttpPost("submit-payment-notification-vtuNation")]
    public async Task<ActionResult<SubmitPaymentNotificationVtuNationResponse>> SubmitPaymentNotificationVtuNation([FromBody] SubmitPaymentNotificationRequestVtuNation submitPaymentNotificationRequestVtuNation)
    {
        var result = await Mediator.Send(new SubmitPaymentNotificationVtuNationCommand() { SubmitPaymentNotificationRequestVtuNation = submitPaymentNotificationRequestVtuNation });

        return Ok(result);
    }


    [HttpGet("get-fund-notification-vtuNation")]
    public async Task<ActionResult<GetFundNotificationsVtuNationResponse>> GetFundNotificationsVtuNation()
    {
        var result = await Mediator.Send(new GetFundNotificationsVtuNationQuery());

        return Ok(result);
    }


    [HttpGet("get-payment-account-info-vtuNation")]
    public async Task<ActionResult<GetPaymentAccountInfoVtuNationResponse>> GetPaymentAccountInfoVtuNation()
    {
        var result = await Mediator.Send(new GetPaymentAccountInfoVtuNationQuery());

        return Ok(result);
    }


    // TRANSACTIONS
    [HttpGet("get-single-transaction-vtuNation/{id}")]
    public async Task<ActionResult<GetSingleTransactionVtuNationResponse>> GetSingleTransactionVtuNation(string id)
    {
        var result = await Mediator.Send(new GetSingleTransactionVtuNationQuery() { Id = id});

        return Ok(result);
    }


    [HttpGet("get-transaction-history-vtuNation")]
    public async Task<ActionResult<GetTransactionHistoryVtuNationResponse>> GetTransactionHistoryVtuNation()
    {
        var result = await Mediator.Send(new GetTransactionHistoryVtuNationQuery());

        return Ok(result);
    }

}
