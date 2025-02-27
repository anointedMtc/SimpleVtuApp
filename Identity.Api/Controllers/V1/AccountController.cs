namespace Identity.Api.Controllers.V1;

[ApiVersion("1.0")]
public class AccountController : ApiBaseController
{

    [HttpPost("register")]
    public async Task<ActionResult<RegisterUserResponse>> RegisterUser([FromBody] ApplicationUserRegisterationRequestDto applicationUserRegisterationRequestDto)
    {
        var result = await Mediator.Send(new RegisterUserCommand() { UserForRegisteration = applicationUserRegisterationRequestDto });

        return Ok(result);
    }


    [HttpPost("emailConfirmation")]
    public async Task<ActionResult<EmailConfirmationResponse>> EmailConfirmation([FromQuery] EmailConfirmationRequestDto emailConfirmationRequestDto)
    {
        var result = await Mediator.Send(new EmailConfirmationCommand() { EmailConfirmation = emailConfirmationRequestDto });

        return Ok(result);
    }


    [HttpPost("resend-EmailConfirmation")]
    public async Task<ActionResult<ResendEmailConfirmationResponse>> ResendEmailConfirmation(string email)
    {
        var result = await Mediator.Send(new ResendEmailConfirmationCommand() { Email = email });

        return Ok(result);
    }


    [HttpPost("login")]
    public async Task<ActionResult<AuthenticateUserResponse>> AuthenticateUser([FromBody] LoginRequestDto loginRequest)
    {
        var result = await Mediator.Send(new AuthenticateUserCommand() { UserForAuthentication = loginRequest });

        return Ok(result);
    }


    [HttpPost("verityTwoFacAuth")]
    public async Task<ActionResult<VerifyTwoFacAuthResponse>> VerifyTwoFacAuth([FromBody] VerifyTwoFacAuthRequestDto verifyTwoFacAuthRequestDto)
    {
        var result = await Mediator.Send(new VerifyTwoFacAuthCommand() { TwoFactorDto = verifyTwoFacAuthRequestDto });

        return Ok(result);
    }


    [HttpPost("refresh-Token")]
    public async Task<ActionResult<RefreshTokenResponse>> RefreshToken([FromBody] RefreshTokenRequestDto refreshTokenRequestDto)
    {
        var result = await Mediator.Send(new RefreshTokenCommand() { RefreshTokenRequest = refreshTokenRequestDto });

        return Ok(result);
    }


    [HttpPost("forgotPassword")]
    public async Task<ActionResult<ForgotPasswordResponse>> ForgotPassword([FromBody] ForgotPasswordRequestDto forgotPasswordRequestDto)
    {
        var result = await Mediator.Send(new ForgotPasswordCommand() { ForgotPasswordDto = forgotPasswordRequestDto });

        return Ok(result);
    }


    [HttpPost("resetPassword")]
    public async Task<ActionResult<ResetPasswordResponse>> ResetPassword([FromBody] ResetPasswordRequestDto resetPasswordRequestDto)
    {
        var result = await Mediator.Send(new ResetPasswordCommand() { ResetPasswordDto = resetPasswordRequestDto });

        return Ok(result);
    }



    // change email
    [HttpPost("change-email-request")]
    [Authorize]
    public async Task<ActionResult<ChangeEmailRequestResponse>> ChangeEmailRequest([FromBody] ChangeEmailRequestDto changeEmailRequestDto)
    {
        var result = await Mediator.Send(new ChangeEmailRequestCommand() { ChangeEmailRequestDto = changeEmailRequestDto });

        return Ok(result);
    }


    [HttpPost("confirm-change-email")]
    [Authorize]
    public async Task<ActionResult<ChangeEmailConfirmationResponse>> ChangeEmailConfirmation([FromBody] ChangeEmailConfirmationRequestDto changeEmailConfirmationRequestDto)
    {
        var result = await Mediator.Send(new ChangeEmailConfirmationCommand() { ChangeEmailConfirmationRequestDto = changeEmailConfirmationRequestDto });

        return Ok(result);
    }


    [HttpPost("Disable-Or-Enable-Two-FacAuth")]
    [Authorize]
    public async Task<ActionResult<DisableOrEnableTwoFacAuthResponse>> DisableOrEnableTwoFacAuth(bool IsTwoFacAuthEnabled)
    {
        var result = await Mediator.Send(new DisableOrEnableTwoFacAuthCommand() { IsTwoFacAuthEnabled = IsTwoFacAuthEnabled });

        return Ok(result);
    }


    // Change password
    [HttpPut("change-my-password")]
    [Authorize]
    public async Task<ActionResult<ChangePasswordResponse>> ChangeMyPassword([FromBody] ChangePasswordRequestDto changePasswordRequestDto)
    {
        var result = await Mediator.Send(new ChangePasswordCommand() { ChangePasswordRequestDto = changePasswordRequestDto });

        return Ok(result);
    }


    // Update User
    [HttpPut("update-my-details")]
    [Authorize]
    public async Task<ActionResult<UpdateUserDetailsResponse>> UpdateMyDetails([FromBody] UpdateUserRequestDto updateUserRequestDto)
    {
        var result = await Mediator.Send(new UpdateUserDetailsCommand() { UpdateUserRequestDto = updateUserRequestDto });

        return Ok(result);
    }


    // GetMyDetails
    [HttpGet("get-My-Details")]
    [Authorize]
    public async Task<ActionResult<GetMyDetailsResponse>> GetMyDetails()
    {
        // just for caching logic... 
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value;

        var result = await Mediator.Send(new GetMyDetailsQuery() { UserId = userId });

        return Ok(result);
    }

}
