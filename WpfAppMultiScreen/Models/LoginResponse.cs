using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioscoAutogestion.Baluma.Casino.App.Models
{
    public class LoginResponse
    {
        public int UserNumber { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public List<int> Permissions { get; set; }

        public bool ForceChangePassword { get; set; }
    }

}
