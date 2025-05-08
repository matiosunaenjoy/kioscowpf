using KioscoAutogestion.Baluma.Casino.App.Arguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioscoAutogestion.Baluma.Casino.App.Services.Readers
{
    public class NFCReaderService : INFCReaderService
    {
        private bool _isReading;
        private System.Threading.Timer _simulationTimer;

        public event EventHandler<NFCCardEventArgs> CardDetected;

        public bool IsReading => _isReading;

        public void StartReading()
        {
            if (_isReading)
                return;

            _isReading = true;

            // En una implementación real, aquí se inicializaría el hardware del lector
            _simulationTimer = new System.Threading.Timer(SimulateNFCRead, null,
                System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);

            // Descomentar para simulación
            // _simulationTimer.Change(9000, System.Threading.Timeout.Infinite);
        }

        public void StopReading()
        {
            if (!_isReading)
                return;

            _isReading = false;

            _simulationTimer?.Dispose();
            _simulationTimer = null;
        }

        private void SimulateNFCRead(object state)
        {
            // Simular la lectura de una tarjeta NFC
            CardDetected?.Invoke(this, new NFCCardEventArgs
            {
                NFCData = "NFC:A1B2C3D4E5F6"
            });
        }
    }
}
