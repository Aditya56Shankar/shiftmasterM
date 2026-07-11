using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ShiftMaster.IdentityService.Application.Services // Update to your preferred namespace location
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

            if (context != null && context.Request.Headers.TryGetValue("Authorization", out var token))
            {
                request.Headers.Authorization = AuthenticationHeaderValue.Parse(token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}