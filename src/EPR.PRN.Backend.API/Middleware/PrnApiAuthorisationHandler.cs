using Azure.Core;
using Azure.Identity;
using EPR.PRN.Backend.API.Configs;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;

namespace EPR.PRN.Backend.API.Middleware
{
    [ExcludeFromCodeCoverage]
    public class PrnApiAuthorisationHandler : DelegatingHandler
    {
        private readonly TokenRequestContext _tokenRequestContext;
        private readonly DefaultAzureCredential? _credentials;

        public PrnApiAuthorisationHandler(IOptions<PrnApiOptions> options)
        {
            if (!string.IsNullOrEmpty(options.Value.ClientId))
            {
                _tokenRequestContext = new TokenRequestContext([options.Value.ClientId]);
                _credentials = new DefaultAzureCredential();
            }
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await AddDefaultToken(request, cancellationToken);
            return await base.SendAsync(request, cancellationToken);
        }

        private async Task AddDefaultToken(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_credentials != null)
            {
                var tokenResult = await _credentials.GetTokenAsync(_tokenRequestContext, cancellationToken);
                request.Headers.Authorization = new AuthenticationHeaderValue(Common.Constants.WebConstants.Header.Bearer, tokenResult.Token);
            }
        }
    }
}