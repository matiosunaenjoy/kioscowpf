using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioscoAutogestion.Baluma.Casino.App.Arguments
{
    public class CardReadEventArgs : EventArgs
    {
        public string CardData { get; set; }
    }
}
