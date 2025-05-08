using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioscoAutogestion.Baluma.Casino.App.Models
{
    public class LoginRequest
    {
        public string Email { get; set; }
        public string UserPassword { get; set; }
        public int SystemId { get; set; }
    }

}
