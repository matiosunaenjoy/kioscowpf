using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioscoAutogestion.Baluma.Casino.App.Models
{
    public class PlayerDataResponse
    {
        public string Status { get; set; }
        public PlayerDataWrapper Data { get; set; }
        public object Error { get; set; }
    }
}
