using KioscoAutogestion.Baluma.Casino.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioscoAutogestion.Baluma.Casino.App.Services.Auth
{
    //public class MockAuthenticationService : IAuthenticationService
    //{
    //    private readonly Dictionary<string, ClientInfo> _fakeDb = new();
    //    public Task<ClientInfo> AuthenticateAsync(string clientNumber, string pin)
    //    {
    //        // Validación simple ficticia
    //        if (clientNumber == "12345" && pin == "0000")
    //        {
    //            return Task.FromResult(new ClientInfo
    //            {
    //                ClientNumber = "12345",
    //                PlayerId = "Player001",
    //                AccumulatedPoints = 1200,
    //                Promotions = new List<string> { "2x1 en bebidas", "50% en tragamonedas" }
    //            });
    //        }

    //        return Task.FromResult<ClientInfo>(null); // simula login fallido
    //    }
    //    public Task<bool> RegisterAsync(NewClientInfo newClient)
    //    {
    //        if (_fakeDb.ContainsKey(newClient.ClientNumber))
    //            return Task.FromResult(false); // Ya existe

    //        var newInfo = new ClientInfo
    //        {
    //            ClientNumber = newClient.ClientNumber,
    //            PlayerId = Guid.NewGuid().ToString(),
    //            AccumulatedPoints = 0,
    //            Promotions = new List<string>(),
    //            Pin = newClient.Pin // (opcional si lo agregás al modelo original)
    //        };

    //        _fakeDb[newClient.ClientNumber] = newInfo;
    //        return Task.FromResult(true);
    //    }

    //    public Task<bool> RegisterAsync(string clientNumber, string pin)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
    //public class MockAuthenticationService : IAuthenticationService
    //{
    //    private readonly ISessionService _session;

    //    public MockAuthenticationService(ISessionService session)
    //    {
    //        _session = session;
    //    }

    //    public Task<bool> AuthenticateAsync(string email, string password)
    //    {
    //        // Simula siempre un login exitoso
    //        var fakeClient = new ClientInfo
    //        {
    //            Email = email,
    //            Token = "mock-token",
    //            RefreshToken = "mock-refresh",
    //            UserNumber = 123,
    //            Permissions = new List<int> { 1, 2, 3 },
    //            ForceChangePassword = false
    //        };

    //        // Guarda en la sesión
    //        _session.CurrentClient = fakeClient;

    //        return Task.FromResult(true);
    //    }

    //    public Task<bool> RegisterAsync(string email, string password)
    //    {
    //        // Simula siempre un registro exitoso
    //        return Task.FromResult(true);
    //    }
    //}
}