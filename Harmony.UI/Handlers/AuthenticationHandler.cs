using System.Net;
using System.Net.Http.Headers;
using Harmony.UI.Services;

namespace Harmony.UI.Handlers;

public class AuthenticationHandler : DelegatingHandler
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IConfiguration _configuration;
    private bool Refreshing;

    public AuthenticationHandler(IAuthenticationService authenticationService, IConfiguration configuration)
    {
        _authenticationService = authenticationService;
        _configuration = configuration;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        string jwt = await _authenticationService.GetJwtAsync();

        bool isToServer = request.RequestUri?.AbsoluteUri.StartsWith(_configuration["HarmonyURL"] ?? throw new NullReferenceException()) ?? false;

        if (isToServer && !string.IsNullOrEmpty(jwt))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        }

        HttpResponseMessage response =  await base.SendAsync(request, cancellationToken);

        if (!Refreshing && !string.IsNullOrEmpty(jwt) && response.StatusCode == HttpStatusCode.Unauthorized)
        {
            try
            {
                Refreshing = true;

                if(await _authenticationService.RefreshAsync())
                {
                    jwt = await _authenticationService.GetJwtAsync();

                    if (isToServer && !string.IsNullOrEmpty(jwt))
                    {
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                    }

                    response = await base.SendAsync(request, cancellationToken);
                }
            }
            finally
            {
                Refreshing = false;
            }
        }

        return response;
    }
}