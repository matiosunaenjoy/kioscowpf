using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioscoAutogestion.Baluma.Casino.App.Models
{
    public class ClientInfo
    {

        public string Email { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public int UserNumber { get; set; }
        public List<int> Permissions { get; set; }
        public bool ForceChangePassword { get; set; }

        // Si por compatibilidad quieres un alias:
        public string ClientNumber
        {
            get => UserNumber.ToString();
            set
            {
                if (int.TryParse(value, out var num))
                    UserNumber = num;
            }
        }
    }
}
