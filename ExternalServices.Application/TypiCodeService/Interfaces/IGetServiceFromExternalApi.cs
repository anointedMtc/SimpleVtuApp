using ExternalServices.Application.TypiCodeService.Models;
using Refit;

namespace ExternalServices.Application.TypiCodeService.Interfaces;

public interface IGetServiceFromExternalApi
{

    [Get("/posts/{id}")]
    Task<ApiResponse<TypiCodePost>> GetPostAsync(int id);

    [Get("/posts")]
    Task<ApiResponse<List<TypiCodePost>>> GetAllPostsAsync();

    [Post("/posts")]
    Task<ApiResponse<TypiCodePost>> CreatePostAsync([Body] TypiCodePost typiCodePost);

    [Put("/posts/{id}")]
    Task<ApiResponse<TypiCodePost>> UpdatePostAsync(int id, [Body] TypiCodePost typiCodePost);

    [Delete("/posts/{id}")]
    Task DeletePostAsync(int id);
}
