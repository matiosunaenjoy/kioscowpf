using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioscoAutogestion.Baluma.Casino.App.Arguments
{
    public class QRCodeReadEventArgs : EventArgs
    {
        public string QRCodeData { get; set; }
    }
}
