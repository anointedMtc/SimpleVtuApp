namespace ExternalServices.Application.TypiCodeService.Interfaces;

public interface IAuthTokenStoreExternalApi
{
    Task<string> GetExterlApiToken();
}
