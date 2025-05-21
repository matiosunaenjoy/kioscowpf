using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KioscoAutogestion.Baluma.Casino.App.Models
{
    public class CurrentNextCategory
    {
        [JsonPropertyName("currentCategory")]
        public string CurrentCategory { get; set; }

        [JsonPropertyName("currentCategoryPointsFrom")]
        public int CurrentCategoryPointsFrom { get; set; }

        [JsonPropertyName("currentCategoryPointsTo")]
        public int CurrentCategoryPointsTo { get; set; }

        [JsonPropertyName("currentCategoryPointsForChange")]
        public int CurrentCategoryPointsForChange { get; set; }

        [JsonPropertyName("nextCategory")]
        public string NextCategory { get; set; }

        [JsonPropertyName("nextCategoryPointsFrom")]
        public int NextCategoryPointsFrom { get; set; }

        [JsonPropertyName("nextCategoryPointsTo")]
        public int NextCategoryPointsTo { get; set; }

        [JsonPropertyName("nextCategoryPointsForChange")]
        public int NextCategoryPointsForChange { get; set; }
    }
}
