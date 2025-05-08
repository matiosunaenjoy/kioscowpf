using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioscoAutogestion.Baluma.Casino.App.Models
{
    public class PlayerResponse
    {
        public string Acct { get; set; }
        public string FirstName { get; set; }
        public string DisplayName { get; set; }
        public string Category { get; set; }
        public bool HasUncompletedActiveSurveys { get; set; }
        public string CategoryName { get; set; }
        public Balances Balances { get; set; }
        public CategoryPoints CategoryPoints { get; set; }
        public CurrentNextCategory CurrentNextCategory { get; set; }
    }
}
