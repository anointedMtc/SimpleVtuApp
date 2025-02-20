using ExternalServices.Application.TypiCodeService.Models;
using Refit;

namespace ExternalServices.Application.TypiCodeService.Interfaces;

public interface IGetJWTFromExternalApiTwo
{
    [Get("/users/{id}")]
    Task<ApiResponse<TypiCodeUser>> GetUser(int id);
}
