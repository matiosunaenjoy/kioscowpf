using KioscoAutogestion.Baluma.Casino.App.Arguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioscoAutogestion.Baluma.Casino.App.Services.Readers
{
    public class QRCodeReaderService : IQRCodeReaderService
    {
        private bool _isReading;
        private System.Threading.Timer _simulationTimer;

        public event EventHandler<QRCodeReadEventArgs> CodeRead;

        public bool IsReading => _isReading;

        public void StartReading()
        {
            if (_isReading)
                return;

            _isReading = true;

            // En una implementación real, aquí se inicializaría el hardware del lector
            _simulationTimer = new System.Threading.Timer(SimulateQRRead, null,
                System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);

            // Descomentar para simulación
             _simulationTimer.Change(7000, System.Threading.Timeout.Infinite);
        }

        public void StopReading()
        {
            if (!_isReading)
                return;

            _isReading = false;

            _simulationTimer?.Dispose();
            _simulationTimer = null;
        }

        private void SimulateQRRead(object state)
        {
            // Simular la lectura de un código QR
            CodeRead?.Invoke(this, new QRCodeReadEventArgs
            {
                QRCodeData = "USER:123456;TOKEN:ABCDEF123456"
            });
        }

        public void InjectQrData(string raw)
        {
            // dispara el evento como si viniera del hardware
            CodeRead?.Invoke(this, new QRCodeReadEventArgs { QRCodeData = raw });
        }
    }
}
