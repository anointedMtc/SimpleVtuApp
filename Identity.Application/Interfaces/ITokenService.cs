using Identity.Domain.Entities;

namespace Identity.Application.Interfaces;

public interface ITokenService
{
    Task<string> GenerateToken(ApplicationUser user);

    string GenerateRefreshToken();
}
