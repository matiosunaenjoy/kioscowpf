using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KioscoAutogestion.Baluma.Casino.App.Models
{
    public class Balances
    {
        [JsonPropertyName("pointsBal")]
        public int PointsBal { get; set; }
    }
}
