using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ShiftMaster.SchedulingService.Clients // Or your preferred namespace
{
    public class TokenForwardingHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenForwardingHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var context = _httpContextAccessor.HttpContext;

            // Extract the token from the incoming API request headers
            if (context != null && context.Request.Headers.TryGetValue("Authorization", out var token))
            {
                // Attach the token onto the outgoing HttpClient request headers
                request.Headers.Authorization = AuthenticationHeaderValue.Parse(token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}