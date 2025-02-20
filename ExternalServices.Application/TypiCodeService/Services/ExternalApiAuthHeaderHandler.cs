using ExternalServices.Application.TypiCodeService.Interfaces;
using System.Net.Http.Headers;

namespace ExternalServices.Application.TypiCodeService.Services;

public class ExternalApiAuthHeaderHandler : DelegatingHandler
{

    private readonly IAuthTokenStoreExternalApi _authTokenStoreExternalApi;

    public ExternalApiAuthHeaderHandler(IAuthTokenStoreExternalApi authTokenStoreExternalApi)
    {
        _authTokenStoreExternalApi = authTokenStoreExternalApi ?? throw new ArgumentNullException(nameof(authTokenStoreExternalApi));
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // if it is null, then we would send it and let the external api deny us service... 
        // we don't even want to do anything specifically considering the fact that we have used polly for resilience and retries... that should be enough
        var token = _authTokenStoreExternalApi.GetExterlApiToken();

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.ToString());

        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}
