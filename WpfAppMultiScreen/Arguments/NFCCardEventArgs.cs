using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioscoAutogestion.Baluma.Casino.App.Arguments
{
    public class NFCCardEventArgs : EventArgs
    {
        public string NFCData { get; set; }
    }
}
