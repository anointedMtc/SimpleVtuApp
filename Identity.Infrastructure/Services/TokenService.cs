using Identity.Application.Interfaces;
using Identity.Domain.Entities;
using Identity.Infrastructure.Models;
using Identity.Shared.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Identity.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly SymmetricSecurityKey _secretKey;
    private readonly string? _validIssuer;
    private readonly string? _validAudience;
    private readonly double _expires;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<TokenService> _logger;

    public TokenService(IConfiguration configuration, UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
        var jwtSettings = configuration.GetSection("jwtSettings").Get<JwtSettings>();
        if (jwtSettings == null || string.IsNullOrEmpty(jwtSettings.Key))
        {
            throw new InvalidOperationException("JWT secret key is not configured.");
        }

        _secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
        _validIssuer = jwtSettings.ValidIssuer;
        _validAudience = jwtSettings.ValidAudience;
        _expires = jwtSettings.Expires;
    }



    public async Task<string> GenerateToken(ApplicationUser user)
    {
        var signingCredentials = new SigningCredentials(_secretKey, SecurityAlgorithms.HmacSha256);
        var claims = await GetClaimsAsync(user);
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    private async Task<List<Claim>> GetClaimsAsync(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            // Note: Storing many claims or very large claim values can impact performance, especially if these claims are included in the authentication token.

            new Claim(ClaimTypes.Name, user?.UserName ?? string.Empty),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.GivenName, user.FirstName),
            new Claim(ClaimTypes.Surname, user.LastName),
            new Claim(ClaimTypes.Gender, user.Gender),
            new Claim(ClaimTypes.Country, user.Nationality),
            new Claim(ClaimTypes.DateOfBirth, user.DateOfBirth.Value.ToString("yyyy-MM-dd")),
            
            new Claim(AppClaimTypes.CreatedAt, user.CreatedAt.ToString("yyyy-MM-dd")),
            new Claim(AppClaimTypes.LastLogin, user.LastLogin.ToString("yyyy-MM-dd"))

        };

        var roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        return claims;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        return new JwtSecurityToken(
            issuer: _validIssuer,
            audience: _validAudience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_expires),
            signingCredentials: signingCredentials
        );
    }


    public string GenerateRefreshToken()
    {
        // RefreshToken is random numbers.. so we want to generate a certain bytes and it's going to be 64... we want to make it long
        var randomNumber = new byte[64];
        // we are making use of the "using" keyword here because we want to dispose the refresh token when we don't need it... it's helping us to manage unused resources... whenever you use the "using" keyword, that's what it does
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        var refreshToken = Convert.ToBase64String(randomNumber);
        return refreshToken;
    }



}