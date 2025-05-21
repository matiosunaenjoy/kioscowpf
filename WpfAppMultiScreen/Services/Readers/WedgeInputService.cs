using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private static readonly Regex _jwtRegex =
                new Regex(@"^[A-Za-z0-9\-_]+\.[A-Za-z0-9\-_]+\.[A-Za-z0-9\-_]+$", RegexOptions.Compiled);

        // Simplificamos: si empieza por % o ; lo mandamos al mag stripe
        private static readonly Regex _magStripeSimple =
            new Regex(@"^[%;].+[_\?]?$", RegexOptions.Compiled);


        public WedgeInputService(
            IMagneticCardReaderService cardSvc,
            IQRCodeReaderService qrSvc)
        {
            _cardSvc = cardSvc;
            _qrSvc = qrSvc;
        }

        //public void OnTextInput(string text)
        //{
        //    if (text == "\r")
        //    {
        //        var raw = _buf.ToString().Trim();
        //        _buf.Clear();

        //        if (IsJwtFormat(raw))
        //        {
        //            // JWT vía QR
        //            _qrSvc.InjectQrData(raw);
        //            Debug.WriteLine($"numero ingresado: {raw}");

        //        }
        //        else if (IsMagneticFormat(raw))
        //        {
        //            // Cualquier pista magnética
        //            _cardSvc.InjectCardData(raw);
        //            Debug.WriteLine($"numero ingresado: {raw}");
        //        }
        //        else
        //        {
        //            Debug.WriteLine($"WedgeInputService: formato desconocido: {raw}");
        //        }
        //    }
        //    else
        //    {
        //        _buf.Append(text);
        //    }

        public void OnTextInput(string text)
        {
            if (text == "\r")
            {
                var raw = _buf.ToString().Trim();
                _buf.Clear();

                if (_magStripeSimple.IsMatch(raw))
                {
                    Debug.WriteLine("WedgeInputService: detectado mag stripe");
                    _cardSvc.InjectCardData(raw);
                }
                else if (_jwtRegex.IsMatch(raw))
                {
                    Debug.WriteLine("WedgeInputService: detectado JWT como QR");
                    _qrSvc.InjectQrData(raw);
                }
                else
                {
                    Debug.WriteLine($"WedgeInputService: formato desconocido: {raw}");
                }
            }
            else
            {
                _buf.Append(text);
            }
        }
        

        private bool IsMagneticFormat(string s)
        {
            // Cualquier cadena que empiece con '%' (track 1 o 2) o con ';'
            return s.StartsWith("%") || s.StartsWith(";");
        }

        private bool IsJwtFormat(string s)
        {
            // Un JWT estándar: dos puntos y sólo Base64URL + '='
            return s.Count(c => c == '.') == 2
                && Regex.IsMatch(s, @"^[A-Za-z0-9\-_]+\.[A-Za-z0-9\-_]+\.[A-Za-z0-9\-_]+=*$");
        }



    }

}
