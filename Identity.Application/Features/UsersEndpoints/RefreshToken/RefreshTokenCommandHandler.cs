using AutoMapper;
using Identity.Application.Interfaces;
using Identity.Domain.Entities;
using Identity.Shared.DTO;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Identity.Application.Features.UsersEndpoints.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
{
    private readonly ILogger _logger;
    private readonly ITokenService _tokenService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMassTransitService _massTransitService;


    public RefreshTokenCommandHandler(IMapper mapper,
        ILogger<RefreshTokenCommandHandler> logger, ITokenService tokenService,
        UserManager<ApplicationUser> userManager,
        IMassTransitService massTransitService)
    {
        _logger = logger;
        _tokenService = tokenService;
        _userManager = userManager;
        _massTransitService = massTransitService;
    }

    public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var refreshTokenResponse = new RefreshTokenResponse();
        refreshTokenResponse.RefreshTokenResponseDto = new RefreshTokenResponseDto();

        _logger.LogInformation("Refresh token request:{@request}", 
            request.RefreshTokenRequest);

        //using var sha256 = SHA256.Create();     // using helps us to dispose... not necessarily garbage collection, but when the resources is no longer needed it would dispose it
        //var refreshTokenHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(request.RefreshTokenRequest.RefreshToken));
        var refreshTokenHash = SHA256.HashData(Encoding.UTF8.GetBytes(request.RefreshTokenRequest.RefreshToken));
        var hashedRefreshedToken = Convert.ToBase64String(refreshTokenHash);

        var user = _userManager.Users.FirstOrDefault(u => u.RefreshToken == hashedRefreshedToken);
        if (user == null)
        {
            _logger.LogError("Invalid refresh token");
            throw new Exception("Invalid refresh token");
        }

        if (user.RefreshTokenExpiryTime < DateTime.Now)
        {
            _logger.LogError("Refresh token expired for user ID: {userId}", user.Id);
            throw new Exception("Refresh token expired");
        }

        var newAccessToken = await _tokenService.GenerateToken(user);
        _logger.LogInformation("Access token generated successfully");


        refreshTokenResponse.Success = true;
        refreshTokenResponse.Message = $"Refresh token successfully generated";
        refreshTokenResponse.RefreshTokenResponseDto.Token = newAccessToken;
        refreshTokenResponse.RefreshTokenResponseDto.TokenExpiresInSeconds = 3600;
        refreshTokenResponse.RefreshTokenResponseDto.RefreshToken = user.RefreshToken;

        return refreshTokenResponse;
    }
}
