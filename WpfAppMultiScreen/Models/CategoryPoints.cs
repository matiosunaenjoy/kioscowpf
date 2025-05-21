using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KioscoAutogestion.Baluma.Casino.App.Models
{
    public class CategoryPoints
    {
        [JsonPropertyName("firstDay")]
        public DateTime FirstDay { get; set; }

        [JsonPropertyName("lastDay")]
        public DateTime LastDay { get; set; }

        [JsonPropertyName("categoryPoints")]
        public int CategoryPoint { get; set; }
    }
}
