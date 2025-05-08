using KioscoAutogestion.Baluma.Casino.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioscoAutogestion.Baluma.Casino.App.Services.Auth
{
    public interface IAuthenticationService
    {

        Task<ClientInfo?> AuthenticateAsync(string email, string password);

        // Nuevos métodos
        Task<ClientInfo> AuthenticateWithCardAsync(string cardData, string pin);
        Task<ClientInfo> AuthenticateWithQRAsync(string qrData);
        Task<ClientInfo> AuthenticateWithNFCAsync(string nfcData);

        Task<bool> RegisterAsync(string email, string password);

    }
}
