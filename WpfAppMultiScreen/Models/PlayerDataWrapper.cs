using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KioscoAutogestion.Baluma.Casino.App.Models
{
    public class PlayerDataWrapper
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("data")]
        public PlayerDataContainer Data { get; set; }

        [JsonPropertyName("error")]
        public object Error { get; set; }
    }
}
