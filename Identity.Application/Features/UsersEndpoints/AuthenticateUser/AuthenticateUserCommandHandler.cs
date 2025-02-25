using AutoMapper;
using Identity.Application.Exceptions;
using Identity.Application.Interfaces;
using Identity.Domain.Entities;
using Identity.Shared.DTO;
using Identity.Shared.IntegrationEvents;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Identity.Application.Features.UsersEndpoints.AuthenticateUser;

public class AuthenticateUserCommandHandler : IRequestHandler<AuthenticateUserCommand, AuthenticateUserResponse>
{
    private readonly ILogger _logger;
    private readonly ITokenService _tokenService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMassTransitService _massTransitService;


    public AuthenticateUserCommandHandler(
        ILogger<AuthenticateUserCommandHandler> logger, ITokenService tokenService,
        UserManager<ApplicationUser> userManager,
        IMassTransitService massTransitService)
    {
        _logger = logger;
        _tokenService = tokenService;
        _userManager = userManager;
        _massTransitService = massTransitService;
    }
    public async Task<AuthenticateUserResponse> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
    {
        var authenticateUserResponse = new AuthenticateUserResponse();
        authenticateUserResponse.LoginResponse = new LoginResponseDto();


        var user = await _userManager.FindByEmailAsync(request.UserForAuthentication.Email!);
        if (user == null)
        {
            _logger.LogError("Invalid email or password");
            throw new CustomBadRequestException("Invalid email or password");
        }
        if (!await _userManager.IsEmailConfirmedAsync(user) && await _userManager.CheckPasswordAsync(user, request.UserForAuthentication.Password!))
        {
            _logger.LogError("Email is not confirmed");                 // We want to display the “Email not confirmed yet” error message only if the Email is not confirmed AND the user has provided the correct username and password.
            throw new CustomUnauthorizedException("You need to confirm your email before you can login");
        }
        if (!await _userManager.IsEmailConfirmedAsync(user) && !await _userManager.CheckPasswordAsync(user, request.UserForAuthentication.Password!))
        {
            _logger.LogWarning("Fake user trying to access account");
            throw new CustomBadRequestException("Bad Request");
        }
        if (await _userManager.IsLockedOutAsync(user))
        {
            throw new CustomUnauthorizedException("The account is locked out");
        }
        if (!await _userManager.CheckPasswordAsync(user, request.UserForAuthentication.Password!))
        {
            await _userManager.AccessFailedAsync(user);

            if (await _userManager.IsLockedOutAsync(user))
            {
                //var content = $"Your Account is Locked Due to Multiple Invalid login Attempts. If you want to reset the password, " +
                //    $"you can use the Forgot Password link on the Login Page";
                //var message = new EmailMetadata(request.UserForAuthentication.Email!, "Locked out account information", content);
                //await _emailService.Send(message);

                await _massTransitService.Publish(new NotifyUserOfAccountLockOutEvent(
                    user.Email!,
                    user.FirstName)
                );

                throw new CustomUnauthorizedException("Your account is locked out, please try again after sometime or you may reset your password. No retry attempt remaining");
            }

            int remainingAttempt = await _userManager.GetAccessFailedCountAsync(user);
            throw new CustomUnauthorizedException($"Invalid email or password. {3 - remainingAttempt} remaining login attempt");
        }

        if (await _userManager.GetTwoFactorEnabledAsync(user))  // this method returns a boolean value indicating whether the user has two factor authentication enabled or not... if they do...
        {
            var providers = await _userManager.GetValidTwoFactorProvidersAsync(user);
            if (!providers.Contains("Email"))       // if the list does not contain an Email provider...
                throw new CustomUnauthorizedException("Invalid 2-Factor Provider.");

            var tokenFor2Fac = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
            
            //var message = new EmailMetadata(user.Email!, "Authentication token", $"Dear subscriber, <br><br> Kindly use this OTP to complete your login request.<br><br> {tokenFor2Fac} <br><br> But if you didn't request for this, kindly ignore. <br><br> Thanks. <br><br> anointedMtc");
            //await _emailService.Send(message);

            await _massTransitService.Publish(new TwoFacAuthRequestedEvent(
                user.Email!,
                tokenFor2Fac,
                user.FirstName)
            );

            authenticateUserResponse.LoginResponse.Is2FactorRequired = user.TwoFactorEnabled;
            authenticateUserResponse.LoginResponse.Provider = "Email";
            authenticateUserResponse.LoginResponse.IsAuthSuccessful = true;
            authenticateUserResponse.LoginResponse.TokenExpiresInSeconds = 7200;
            authenticateUserResponse.Success = true;
            authenticateUserResponse.Message = $"Kindly check your email for your OTP token which you can use to complete the login request";

            return authenticateUserResponse;

        }

        var token = await _tokenService.GenerateToken(user);

        var refreshToken = _tokenService.GenerateRefreshToken();

        using var sha256 = SHA256.Create();     // remember that when we generated the token, we generated it in a kind of hash format so when we want to save it, we want to also hash and save it... 
        var refreshTokenHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(refreshToken));    // hash it for security reasons... and when we want to also get it, we un-hash it / decrypt it... so that incase someone interfares he/she would only get the hashed one which would be meaningless to him/her
        user.RefreshToken = Convert.ToBase64String(refreshTokenHash);
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);      // or 2days

        user.LastLogin = DateTime.UtcNow;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            _logger.LogError("Failed to update user: {errors}", errors);
            throw new Exception($"Failed to update user: {errors}");
        }

        await _userManager.ResetAccessFailedCountAsync(user);       // right now after the lockout action, the user can reset the password by clicking the forgotpassword link on the Login Page... But even then, the user must wait for the lockout to expire to be able to login again... this is also somehting we would want to change... we want to enable the account as soon as the password reset action is completed to do that, let's modify the resetpassword action method


        authenticateUserResponse.Success = true;
        authenticateUserResponse.Message = "Login successful";
        authenticateUserResponse.LoginResponse.IsAuthSuccessful = true;
        authenticateUserResponse.LoginResponse.Token = token;
        authenticateUserResponse.LoginResponse.RefreshToken = refreshToken;
        authenticateUserResponse.LoginResponse.TokenExpiresInSeconds = 3600;

        return authenticateUserResponse;
    }


}
