using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioscoAutogestion.Baluma.Casino.App.Models
{
    public class CurrentNextCategory
    {
        public string CurrentCategory { get; set; }
        public int CurrentCategoryPointsFrom { get; set; }
        public int CurrentCategoryPointsTo { get; set; }
        public int CurrentCategoryPointsForChange { get; set; }
        public string NextCategory { get; set; }
        public int NextCategoryPointsFrom { get; set; }
        public int NextCategoryPointsTo { get; set; }
        public int NextCategoryPointsForChange { get; set; }
    }
}
