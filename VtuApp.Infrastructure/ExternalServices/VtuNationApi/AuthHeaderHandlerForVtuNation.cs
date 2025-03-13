using System.Net.Http.Headers;
using VtuApp.Application.Interfaces.ExternalServices.VtuNationApi;

namespace VtuApp.Infrastructure.ExternalServices.VtuNationApi;

public class AuthHeaderHandlerForVtuNation : DelegatingHandler
{
    private readonly ITokenStoreForVtuNation _tokenStoreForVtuNation;

    public AuthHeaderHandlerForVtuNation(ITokenStoreForVtuNation tokenStoreForVtuNation)
    {
        _tokenStoreForVtuNation = tokenStoreForVtuNation ?? throw new ArgumentNullException(nameof(tokenStoreForVtuNation));
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // if it is null, then we would send it and let the external api deny us service... 
        // we don't even want to do anything specifically considering the fact that we have used polly for resilience and retries... that should be enough
        var token = await _tokenStoreForVtuNation.GetVtuNationApiToken();

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}
