using Identity.Application.Exceptions;
using Identity.Application.Interfaces;
using Identity.Domain.Entities;
using Identity.Shared.DTO;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace Identity.Application.Features.UsersEndpoints.VerifyTwoFacAuth;

public class VerifyTwoFacAuthCommandHandler : IRequestHandler<VerifyTwoFacAuthCommand, VerifyTwoFacAuthResponse>
{
    private readonly ILogger _logger;
    private readonly ITokenService _tokenService;
    private readonly UserManager<ApplicationUser> _userManager;


    public VerifyTwoFacAuthCommandHandler(
        ILogger<VerifyTwoFacAuthCommandHandler> logger, ITokenService tokenService,
        UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _tokenService = tokenService;
        _userManager = userManager;
    }
    public async Task<VerifyTwoFacAuthResponse> Handle(VerifyTwoFacAuthCommand request, CancellationToken cancellationToken)
    {
        var verifyTwoFacAuthResponse = new VerifyTwoFacAuthResponse();
        verifyTwoFacAuthResponse.VerifyTwoFacAuthResponseDto = new VerifyTwoFacAuthResponseDto();

        var user = await _userManager.FindByEmailAsync(request.TwoFactorDto.Email!);
        if (user is null)
        {
            throw new CustomBadRequestException("Invalid Request");
        }

        try
        {
            await _userManager.VerifyTwoFactorTokenAsync(user, _userManager.Options.Tokens.AuthenticatorTokenProvider, request.TwoFactorDto.Token!);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while processing {typeofRequest} for User with Id {UserId} due to error: {ex.Message}",
                typeof(VerifyTwoFacAuthCommand),
                request.TwoFactorDto.Email,
                ex.Message);

            verifyTwoFacAuthResponse.Success = false;
            verifyTwoFacAuthResponse.Message = "Invalid authenticator code entered";

            throw new CustomBadRequestException();
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


        verifyTwoFacAuthResponse.Success = true;
        verifyTwoFacAuthResponse.Message = "Login successful";
        verifyTwoFacAuthResponse.VerifyTwoFacAuthResponseDto.IsAuthSuccessful = true;
        verifyTwoFacAuthResponse.VerifyTwoFacAuthResponseDto.Token = token;
        verifyTwoFacAuthResponse.VerifyTwoFacAuthResponseDto.RefreshToken = refreshToken;
        verifyTwoFacAuthResponse.VerifyTwoFacAuthResponseDto.TokenExpiresInSeconds = 3600;

        return verifyTwoFacAuthResponse;
    }
}
