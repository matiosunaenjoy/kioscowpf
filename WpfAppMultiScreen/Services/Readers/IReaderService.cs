using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioscoAutogestion.Baluma.Casino.App.Services.Readers
{
    public interface IReaderService
    {
        bool IsReading { get; }
        void StartReading();
        void StopReading();
    }
}
