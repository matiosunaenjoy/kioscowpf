
using KioscoAutogestion.Baluma.Casino.App.Models;
using KioscoAutogestion.Baluma.Casino.App.Services.Player;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace KioscoAutogestion.Baluma.Casino.App.Services.Auth
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _http;
        private readonly ISessionService _session;

        public AuthenticationService(HttpClient http, ISessionService session)
        {
            _http = http;
            _session = session;
        }

        public async Task<ClientInfo?> AuthenticateAsync(string email, string password)
        {
            var request = new LoginRequest
            {
                Email = email,
                UserPassword = password,
                SystemId = 16
            };

            var response = await _http.PostAsJsonAsync("/authenticationService/Api/auth/login", request);
            if (!response.IsSuccessStatusCode)
                return null;

            var lr = await response.Content.ReadFromJsonAsync<LoginResponse>();
            if (lr == null || string.IsNullOrEmpty(lr.AccessToken))
                return null;

            var client = new ClientInfo
            {
                Email = email,
                Token = lr.AccessToken,
                RefreshToken = lr.RefreshToken,
                UserNumber = lr.UserNumber,
                Permissions = lr.Permissions,
                ForceChangePassword = lr.ForceChangePassword
            };
            _session.CurrentClient = client;
            return client;
        }

        public async Task<ClientInfo?> AuthenticateWithCardAsync(string cardData, string pin)
        {
            // Ejemplo de request para tarjeta magnética
            var request = new
            {
                CardData = cardData,
                Pin = pin,
                SystemId = 16
            };

            var response = await _http.PostAsJsonAsync("/authenticationService/Api/auth/card", request);
            if (!response.IsSuccessStatusCode)
                return null;

            var lr = await response.Content.ReadFromJsonAsync<LoginResponse>();
            if (lr == null || string.IsNullOrEmpty(lr.AccessToken))
                return null;

            var client = new ClientInfo
            {
                Token = lr.AccessToken,
                RefreshToken = lr.RefreshToken,
                UserNumber = lr.UserNumber,
                Permissions = lr.Permissions,
                ForceChangePassword = lr.ForceChangePassword
            };
            _session.CurrentClient = client;
            return client;
        }

        public async Task<ClientInfo?> AuthenticateWithNFCAsync(string nfcData)
        {
            // Ejemplo de request para NFC
            var request = new
            {
                NfcPayload = nfcData,
                SystemId = 16
            };

            var response = await _http.PostAsJsonAsync("/authenticationService/Api/auth/nfc", request);
            if (!response.IsSuccessStatusCode)
                return null;

            var lr = await response.Content.ReadFromJsonAsync<LoginResponse>();
            if (lr == null || string.IsNullOrEmpty(lr.AccessToken))
                return null;

            var client = new ClientInfo
            {
                Token = lr.AccessToken,
                RefreshToken = lr.RefreshToken,
                UserNumber = lr.UserNumber,
                Permissions = lr.Permissions,
                ForceChangePassword = lr.ForceChangePassword
            };
            _session.CurrentClient = client;
            return client;
        }

        public async Task<ClientInfo?> AuthenticateWithQRAsync(string qrData)
        {
            // Parseo simple: "USER:123;TOKEN:XYZ"
            string? userId = null, token = null;
            foreach (var part in qrData.Split(';'))
            {
                if (part.StartsWith("USER:")) userId = part.Substring(5);
                if (part.StartsWith("TOKEN:")) token = part.Substring(6);
            }

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
                return null;

            // Ejemplo de request para QR
            var request = new
            {
                UserId = userId,
                Token = token,
                SystemId = 16
            };

            var response = await _http.PostAsJsonAsync("/authenticationService/Api/auth/qr", request);
            if (!response.IsSuccessStatusCode)
                return null;

            var lr = await response.Content.ReadFromJsonAsync<LoginResponse>();
            if (lr == null || string.IsNullOrEmpty(lr.AccessToken))
                return null;

            var client = new ClientInfo
            {
                Token = lr.AccessToken,
                RefreshToken = lr.RefreshToken,
                UserNumber = lr.UserNumber,
                Permissions = lr.Permissions,
                ForceChangePassword = lr.ForceChangePassword
            };
            _session.CurrentClient = client;
            return client;
        }

        public Task<bool> RegisterAsync(string email, string password)
            => Task.FromResult(false);
    }
}
