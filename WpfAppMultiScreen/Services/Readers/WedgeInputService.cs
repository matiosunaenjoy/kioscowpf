using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KioscoAutogestion.Baluma.Casino.App.Services.Readers
{
    public class WedgeInputService
    {
        private readonly IMagneticCardReaderService _cardSvc;
        private readonly IQRCodeReaderService _qrSvc;
        private StringBuilder _buf = new StringBuilder();

        public WedgeInputService(
            IMagneticCardReaderService cardSvc,
            IQRCodeReaderService qrSvc)
        {
            _cardSvc = cardSvc;
            _qrSvc = qrSvc;
        }

        public void OnTextInput(string text)
        {
            if (text == "\r")
            {
                var raw = _buf.ToString(); _buf.Clear();
                if (IsMagneticFormat(raw))
                    _cardSvc.InjectCardData(raw);
                else
                    _qrSvc.InjectQrData(raw);
            }
            else
            {
                _buf.Append(text);
            }
        }

        //private bool IsMagneticFormat(string s)
        //{
        //    // patrón ISO 7811: empieza con % y termina con ?
        //    if (Regex.IsMatch(s, @"^%.*\?$")) return true;

        //    // o sólo dígitos y longitud típica (10–20)
        //    if (Regex.IsMatch(s, @"^\d{10,20}$")) return true;

        //    return false;
        //}

        public bool IsMagneticFormat(string s)
        {
            // Ejemplo de detección: ISO-7811 empieza con % y termina con ?
            //if (System.Text.RegularExpressions.Regex.IsMatch(s, @"^%.*\?$"))
            //    return true;
            //// O sólo dígitos típicos de una tarjeta:
            //if (System.Text.RegularExpressions.Regex.IsMatch(s, @"^\d{10,20}$"))
            //    return true;
            //return false;
            return s.StartsWith("%") || s.StartsWith(";");
        }

    }

}
