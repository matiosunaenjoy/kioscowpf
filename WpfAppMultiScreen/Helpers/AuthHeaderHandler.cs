using KioscoAutogestion.Baluma.Casino.App.Services.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KioscoAutogestion.Baluma.Casino.App.Helpers
{
    public class AuthHeaderHandler : DelegatingHandler
    {
        private readonly ISessionService _session;

        public AuthHeaderHandler(ISessionService session)
        {
            _session = session;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = _session.CurrentClient?.Token;
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
