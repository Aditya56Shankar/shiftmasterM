using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ShiftMaster.CommsAuditService.Clients // Adjust namespace to match your current project structure
{
    internal class TokenForwardingHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenForwardingHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var context = _httpContextAccessor.HttpContext;

            // 1. Check if there is an active HTTP request context running
            if (context != null && context.Request.Headers.TryGetValue("Authorization", out var token))
            {
                // 2. Extract the incoming JWT token string and attach it to the outgoing HttpClient headers
                request.Headers.Authorization = AuthenticationHeaderValue.Parse(token);
            }

            // 3. Pass execution down to the next handler in the HttpClient pipeline
            return await base.SendAsync(request, cancellationToken);
        }
    }
}