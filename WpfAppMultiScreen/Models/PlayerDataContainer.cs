using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KioscoAutogestion.Baluma.Casino.App.Models
{
    public class PlayerDataContainer
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }

        // Cambia PlayerDataResponse por PlayerResponse
        [JsonPropertyName("response")]
        public PlayerResponse Response { get; set; }
    }
}
