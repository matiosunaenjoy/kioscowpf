using KioscoAutogestion.Baluma.Casino.App.Arguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioscoAutogestion.Baluma.Casino.App.Services.Readers
{
    public interface INFCReaderService:IReaderService
    {
        event EventHandler<NFCCardEventArgs> CardDetected;
        
    }

   
}
