using KioscoAutogestion.Baluma.Casino.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioscoAutogestion.Baluma.Casino.App.Services.Player
{
    public class SessionService : ISessionService
    {
        public ClientInfo CurrentClient { get;  set; }

        public void SetSession(ClientInfo client)
        {
            CurrentClient = client;
        }

        public void ClearSession()
        {
            CurrentClient = null;
        }
    }
}
