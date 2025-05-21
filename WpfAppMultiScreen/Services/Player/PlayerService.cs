using KioscoAutogestion.Baluma.Casino.App.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace KioscoAutogestion.Baluma.Casino.App.Services.Player
{
    
    public class PlayerService : IPlayerService
    {
        private readonly HttpClient _http;
        private readonly ISessionService _session;

        public PlayerService(HttpClient http, ISessionService session)
        {
            _http = http;
            _session = session;
            _http.BaseAddress = new Uri("https://svc.baluma.com.uy/");
        }



        public async Task<PlayerResponse> GetPlayerDataAsync()
        {
            var token = _session.CurrentClient?.Token?.Trim();
            if (string.IsNullOrEmpty(token))
                return null;

            using var req = new HttpRequestMessage(
                HttpMethod.Get,
                "/playerService/api/player/getplayerdata");
            req.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
            req.Headers.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var resp = await _http.SendAsync(req);
            if (!resp.IsSuccessStatusCode)
                return null;

            var wrapper = await resp.Content
                .ReadFromJsonAsync<PlayerDataWrapper>();
            if (wrapper == null || wrapper.Status != "success")
                return null;

            // Aquí `wrapper.Data.Response` ya es un PlayerResponse
            return wrapper.Data.Response;
        }


    }
}
