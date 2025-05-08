using KioscoAutogestion.Baluma.Casino.App.Arguments;
using KioscoAutogestion.Baluma.Casino.App.Services.Readers;
using System.IO.Ports;

public class MagneticCardReaderService : IMagneticCardReaderService
{
    private SerialPort _port;
    private bool _isReading;

    public event EventHandler<CardReadEventArgs> CardRead;
    public bool IsReading => _isReading;

    public void StartReading()
    {
        if (_isReading) return;
        _isReading = true;

        // Configura tu puerto (ajusta COM y baudios a lo que use tu reader)
        _port = new SerialPort("COM4", 9600, Parity.None, 8, StopBits.One);
        _port.DataReceived += Port_DataReceived;
        _port.Open();
    }

    public void StopReading()
    {
        if (!_isReading) return;
        _isReading = false;

        _port.DataReceived -= Port_DataReceived;
        _port.Close();
        _port.Dispose();
        _port = null;
    }

    private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        // Lee todo lo disponible
        var raw = _port.ReadExisting();

        // Aquí parsea tracks si es necesario, por ejemplo:
        // string track = ParseTrack1(raw);
        string track = raw.Trim();

        // Y dispara el evento
        OnCardDataReceived(track);
    }

    public void OnCardDataReceived(string cardData)
    {
        CardRead?.Invoke(this, new CardReadEventArgs { CardData = cardData });
    }

    public void InjectCardData(string cardData)
   {
       CardRead?.Invoke(this, new CardReadEventArgs { CardData = cardData
});    }
}
