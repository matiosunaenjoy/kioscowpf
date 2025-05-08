using KioscoAutogestion.Baluma.Casino.App.Arguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioscoAutogestion.Baluma.Casino.App.Services.Readers
{
    //public interface IQRCodeReaderService:IReaderService
    //{
    //    event EventHandler<QRCodeReadEventArgs> CodeRead;

    //}
    // IQRCodeReaderService.cs
    public interface IQRCodeReaderService
    {
        event EventHandler<QRCodeReadEventArgs> CodeRead;
        bool IsReading { get; }
        void StartReading();
        void StopReading();

        // <<< Añade este método:
        void InjectQrData(string raw);
    }


}
