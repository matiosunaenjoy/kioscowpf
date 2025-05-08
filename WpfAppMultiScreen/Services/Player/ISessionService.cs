using KioscoAutogestion.Baluma.Casino.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioscoAutogestion.Baluma.Casino.App.Services.Player
{
    public interface ISessionService
    {
        //ClientInfo CurrentClient { get; set; }
        //void SetSession(ClientInfo client);
        //void ClearSession();
        //Task<PlayerDataResponse> GetPlayerDataAsync();
        ClientInfo CurrentClient { get; set; }
    }
}
