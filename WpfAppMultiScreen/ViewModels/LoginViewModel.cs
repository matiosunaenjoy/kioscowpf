//using KioscoAutogestion.Baluma.Casino.App.Arguments;
//using KioscoAutogestion.Baluma.Casino.App.Helpers;
//using KioscoAutogestion.Baluma.Casino.App.Models;
//using KioscoAutogestion.Baluma.Casino.App.Services.Auth;
//using KioscoAutogestion.Baluma.Casino.App.Services.Navigation;
//using KioscoAutogestion.Baluma.Casino.App.Services.Player;
//using KioscoAutogestion.Baluma.Casino.App.Services.Readers;
//using KioscoAutogestion.Baluma.Casino.App.Services.Utils;
//using KioscoAutogestion.Baluma.Casino.App.Views.Auth;
//using System;
//using System.Diagnostics;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Input;
//using Application = System.Windows.Application;
//using MessageBox = System.Windows.MessageBox;

//namespace KioscoAutogestion.Baluma.Casino.App.ViewModels
//{
//    public class LoginViewModel : ViewModelBase
//    {
//        private readonly IAuthenticationService _auth;
//        private readonly INavigationService _nav;
//        private readonly IMagneticCardReaderService _magneticCardReader;
//        private readonly IQRCodeReaderService _qrCodeReader;
//        private readonly INFCReaderService _nfcReader;
//        private readonly IDialogService _dialogService;
//        private readonly IPlayerService _playerSvc;

//        private RelayCommandSync _startListeningCommand;
//        private RelayCommandSync _stopListeningCommand;

//        private string _email;
//        private string _password;
//        private bool _isListening;
//        private string _statusMessage;

//        public LoginViewModel(
//            IAuthenticationService auth,
//            IPlayerService playerSvc,
//            INavigationService nav,
//            IMagneticCardReaderService magneticCardReader,
//            IQRCodeReaderService qrCodeReader,
//            INFCReaderService nfcReader,
//            IDialogService dialogService)
//        {
//            _auth = auth;
//            _nav = nav;
//            _magneticCardReader = magneticCardReader;
//            _qrCodeReader = qrCodeReader;
//            _nfcReader = nfcReader;
//            _dialogService = dialogService;
//            _playerSvc = playerSvc;
//            // Comandos de lectura
//            _startListeningCommand = new RelayCommandSync(_ => StartListening(), _ => !IsListening);
//            StartListeningCommand = _startListeningCommand;
//            _stopListeningCommand = new RelayCommandSync(_ => StopListening(), _ => IsListening);
//            StopListeningCommand = _stopListeningCommand;

//            // Comando de login
//            LoginCommand = new RelayCommand(async _ => await DoLoginAsync(), _ => CanLogin);

//            // Comando de inicio de sesión
//            OpenLoginWindowCommand = new RelayCommandSync(_ => _nav.NavigateTo("LoginWindow", screenIndex: 0), _ => true);

//            // Comando de registro
//            OpenRegisterWindowCommand = new RelayCommandSync(_ => _nav.NavigateTo("RegisterWindow", screenIndex: 0), _ => true); 

//            // Suscripción a eventos de lectores
//            _magneticCardReader.CardRead += OnMagneticCardRead;
//            _qrCodeReader.CodeRead += OnQRCodeRead;
//            _nfcReader.CardDetected += OnNFCCardDetected;

//            StatusMessage = "Listo para iniciar la lectura de dispositivos";
//        }

//        #region Propiedades

//        public string Email
//        {
//            get => _email;
//            set
//            {
//                if (_email == value) return;
//                _email = value;
//                OnPropertyChanged();
//                ((RelayCommand)LoginCommand).NotifyCanExecuteChanged();
//            }
//        }

//        public string Password
//        {
//            get => _password;
//            set
//            {
//                if (_password == value) return;
//                _password = value;
//                OnPropertyChanged();
//                ((RelayCommand)LoginCommand).NotifyCanExecuteChanged();
//            }
//        }

//        public bool IsListening
//        {
//            get => _isListening;
//            private set
//            {
//                if (_isListening == value) return;
//                _isListening = value;
//                OnPropertyChanged();
//                _startListeningCommand.NotifyCanExecuteChanged();
//                _stopListeningCommand.NotifyCanExecuteChanged();
//            }
//        }

//        public string StatusMessage
//        {
//            get => _statusMessage;
//            set
//            {
//                if (_statusMessage == value) return;
//                _statusMessage = value;
//                OnPropertyChanged();
//            }
//        }

//        public ICommand LoginCommand { get; }
//        public ICommand OpenLoginWindowCommand { get; }
//        public ICommand OpenRegisterWindowCommand { get; }
//        public ICommand StartListeningCommand { get; }
//        public ICommand StopListeningCommand { get; }

//        private bool CanLogin =>
//            !string.IsNullOrWhiteSpace(Email) &&
//            !string.IsNullOrWhiteSpace(Password);

//        #endregion

//        #region Control de lectura

//        public void StartListening()
//        {
//            if (IsListening) return;
//            IsListening = true;
//            StatusMessage = "Esperando lectura de tarjeta magnética, QR o NFC...";

//            _magneticCardReader.StartReading();
//            _qrCodeReader.StartReading();
//            _nfcReader.StartReading();
//        }

//        public void StopListening()
//        {
//            if (!IsListening) return;
//            IsListening = false;

//            _magneticCardReader.StopReading();
//            _qrCodeReader.StopReading();
//            _nfcReader.StopReading();

//            StatusMessage = "Lectores desactivados";
//        }

//        #endregion

//        #region Handlers de eventos

//        private void OnMagneticCardRead(object sender, CardReadEventArgs e)
//        {
//            _ = HandleMagneticReadAsync(e.CardData);
//        }

//        //private async Task HandleMagneticReadAsync(string data)
//        //{
//        //    try
//        //    {
//        //        StopListening();
//        //        var pinVm = new PinEntryViewModel(data, _auth, _nav);

//        //        // Mostrar diálogo en UI thread
//        //        var op = Application.Current.Dispatcher.InvokeAsync(() =>
//        //            _dialogService.ShowDialogAsync<PinEntryView>(pinVm));
//        //        var dialogTask = await op;
//        //        bool? result = await dialogTask;

//        //        if (result == true && pinVm.IsAuthenticated)
//        //            NavigateToDashboard(pinVm.ClientInfo);
//        //        else
//        //        {
//        //            StatusMessage = "Autenticación con tarjeta cancelada";
//        //            await Task.Delay(2000);
//        //            StartListening();
//        //        }
//        //    }
//        //    catch
//        //    {
//        //        StatusMessage = "Error al procesar tarjeta magnética";
//        //        await Task.Delay(2000);
//        //        StartListening();
//        //    }
//        //}

//        private async Task HandleMagneticReadAsync(string data)
//        {
//            StopListening();

//            // 1) Abrir el modal PIN
//            var pinVm = new PinEntryViewModel(data, _auth, _nav);
//            bool? result = await Application.Current.Dispatcher
//                .InvokeAsync(() => _dialogService.ShowDialogAsync<PinEntryView>(pinVm));

//            if (result != true || !pinVm.IsAuthenticated)
//            {
//                StatusMessage = "Autenticación con tarjeta cancelada";
//                await Task.Delay(2000);
//                StartListening();
//                return;
//            }

//            // 2) Ya autenticado con la tarjeta, obtengo ClientInfo
//            var client = pinVm.ClientInfo;

//            // 3) Cargo datos del jugador
//            PlayerDataResponse playerData;
//            try
//            {
//                playerData = await _playerSvc.GetPlayerDataAsync();
//            }
//            catch
//            {
//                MessageBox.Show(
//                    "No se pudieron cargar los datos del jugador",
//                    "Error",
//                    MessageBoxButton.OK,
//                    MessageBoxImage.Error);
//                StartListening();
//                return;
//            }

//            if (playerData?.Status != "OK")
//            {
//                MessageBox.Show(
//                    "No se pudieron cargar los datos del jugador",
//                    "Error",
//                    MessageBoxButton.OK,
//                    MessageBoxImage.Error);
//                StartListening();
//                return;
//            }

//            // 4) Sólo aquí navego al dashboard, con client + playerData
//            _nav.NavigateTo("DashboardWindow", new { client, playerData }, screenIndex: 0);
//            _nav.NavigateTo("DashboardHome", new { client, playerData });
//        }


//        private void OnQRCodeRead(object sender, QRCodeReadEventArgs e)
//        {
//            Debug.WriteLine("QR recibido: " + e.QRCodeData);

//            _ = HandleQRCodeReadAsync(e.QRCodeData);
//        }

//        private async Task HandleQRCodeReadAsync(string qrData)
//        {
//            try
//            {
//                StopListening();
//                var client = await _auth.AuthenticateWithQRAsync(qrData);
//                if (client != null)
//                    NavigateToDashboard(client);
//                else
//                {
//                    StatusMessage = "Código QR no válido";
//                    await Task.Delay(2000);
//                    StartListening();
//                }
//            }
//            catch
//            {
//                StatusMessage = "Error al procesar QR";
//                await Task.Delay(2000);
//                StartListening();
//            }
//        }




//        private void OnNFCCardDetected(object sender, NFCCardEventArgs e)
//        {
//            _ = HandleNFCReadAsync(e.NFCData);
//        }

//        private async Task HandleNFCReadAsync(string nfcData)
//        {
//            try
//            {
//                StopListening();
//                var client = await _auth.AuthenticateWithNFCAsync(nfcData);
//                if (client != null)
//                    NavigateToDashboard(client);
//                else
//                {
//                    StatusMessage = "Tarjeta NFC no válida";
//                    await Task.Delay(2000);
//                    StartListening();
//                }
//            }
//            catch
//            {
//                StatusMessage = "Error al procesar NFC";
//                await Task.Delay(2000);
//                StartListening();
//            }
//        }

//        #endregion

//        #region Helpers

//        private void NavigateToDashboard(ClientInfo client)
//        {
//            _nav.NavigateTo("DashboardWindow", client, screenIndex: 0);
//            _nav.NavigateTo("DashboardHome", client);
//        }

//        private async Task DoLoginAsync()
//        {
//            try
//            {
//                var client = await _auth.AuthenticateAsync(Email, Password);
//                if (client != null)
//                    NavigateToDashboard(client);
//                else
//                    MessageBox.Show("Credenciales inválidas", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
//            }
//            catch
//            {
//                MessageBox.Show("Error al autenticar", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
//            }
//        }

//        #endregion

//        #region Cleanup


//        public void Cleanup()
//        {
//            _magneticCardReader.CardRead -= OnMagneticCardRead;
//            _qrCodeReader.CodeRead -= OnQRCodeRead;
//            _nfcReader.CardDetected -= OnNFCCardDetected;
//            StopListening();
//        }

//        #endregion
//    }
//}


using KioscoAutogestion.Baluma.Casino.App.Arguments;
using KioscoAutogestion.Baluma.Casino.App.Helpers;
using KioscoAutogestion.Baluma.Casino.App.Models;
using KioscoAutogestion.Baluma.Casino.App.Services.Auth;
using KioscoAutogestion.Baluma.Casino.App.Services.Navigation;
using KioscoAutogestion.Baluma.Casino.App.Services.Player;
using KioscoAutogestion.Baluma.Casino.App.Services.Readers;
using KioscoAutogestion.Baluma.Casino.App.Services.Utils;
using KioscoAutogestion.Baluma.Casino.App.Views.Auth;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace KioscoAutogestion.Baluma.Casino.App.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IAuthenticationService _auth;
        private readonly INavigationService _nav;
        private readonly IMagneticCardReaderService _magneticCardReader;
        private readonly IQRCodeReaderService _qrCodeReader;
        private readonly INFCReaderService _nfcReader;
        private readonly IDialogService _dialogService;
        private readonly IPlayerService _playerSvc;

        private RelayCommandSync _startListeningCommand;
        private RelayCommandSync _stopListeningCommand;

        private string _email;
        private string _password;
        private bool _isListening;
        private string _statusMessage;

        public LoginViewModel(
            IAuthenticationService auth,
            IPlayerService playerSvc,
            INavigationService nav,
            IMagneticCardReaderService magneticCardReader,
            IQRCodeReaderService qrCodeReader,
            INFCReaderService nfcReader,
            IDialogService dialogService)
        {
            _auth = auth;
            _nav = nav;
            _magneticCardReader = magneticCardReader;
            _qrCodeReader = qrCodeReader;
            _nfcReader = nfcReader;
            _dialogService = dialogService;
            _playerSvc = playerSvc;

            _startListeningCommand = new RelayCommandSync(_ => StartListening(), _ => !IsListening);
            _stopListeningCommand = new RelayCommandSync(_ => StopListening(), _ => IsListening);

            StartListeningCommand = _startListeningCommand;
            StopListeningCommand = _stopListeningCommand;
            LoginCommand = new RelayCommand(async _ => await DoLoginAsync(), _ => CanLogin);
            OpenLoginWindowCommand = new RelayCommandSync(_ => _nav.NavigateTo("LoginWindow", screenIndex: 0), _ => true);
            OpenRegisterWindowCommand = new RelayCommandSync(_ => _nav.NavigateTo("RegisterWindow", screenIndex: 0), _ => true);

            _magneticCardReader.CardRead += OnMagneticCardRead;
            _qrCodeReader.CodeRead += OnQRCodeRead;
            _nfcReader.CardDetected += OnNFCCardDetected;

            StatusMessage = "Listo para iniciar la lectura de dispositivos";
        }

        #region Propiedades

        public string Email
        {
            get => _email;
            set
            {
                if (_email == value) return;
                _email = value;
                OnPropertyChanged();
                ((RelayCommand)LoginCommand).NotifyCanExecuteChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (_password == value) return;
                _password = value;
                OnPropertyChanged();
                ((RelayCommand)LoginCommand).NotifyCanExecuteChanged();
            }
        }

        public bool IsListening
        {
            get => _isListening;
            private set
            {
                if (_isListening == value) return;
                _isListening = value;
                OnPropertyChanged();
                _startListeningCommand.NotifyCanExecuteChanged();
                _stopListeningCommand.NotifyCanExecuteChanged();
            }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                if (_statusMessage == value) return;
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand { get; }
        public ICommand OpenLoginWindowCommand { get; }
        public ICommand OpenRegisterWindowCommand { get; }
        public ICommand StartListeningCommand { get; }
        public ICommand StopListeningCommand { get; }

        private bool CanLogin =>
            !string.IsNullOrWhiteSpace(Email) &&
            !string.IsNullOrWhiteSpace(Password);

        #endregion

        #region Control de lectura

        public void StartListening()
        {
            if (IsListening) return;
            IsListening = true;
            StatusMessage = "Esperando lectura de tarjeta magnética, QR o NFC...";

            _magneticCardReader.StartReading();
            _qrCodeReader.StartReading();
            _nfcReader.StartReading();
        }

        public void StopListening()
        {
            if (!IsListening) return;
            IsListening = false;

            _magneticCardReader.StopReading();
            _qrCodeReader.StopReading();
            _nfcReader.StopReading();

            StatusMessage = "Lectores desactivados";
        }

        #endregion

        #region Handlers de eventos

        private async void OnMagneticCardRead(object sender, CardReadEventArgs e)
        {
            await HandleMagneticReadAsync(e.CardData);
        }

        private async Task HandleMagneticReadAsync(string data)
        {
            try
            {
                StopListening();

                var pinVm = new PinEntryViewModel(data, _auth, _nav);
                var dialogTask = Application.Current.Dispatcher.Invoke(() =>
                    _dialogService.ShowDialogAsync<PinEntryView>(pinVm));

                bool? result = await dialogTask;

                if (result == true && pinVm.IsAuthenticated)
                {
                    var playerData = await _playerSvc.GetPlayerDataAsync();
                    if (playerData?.Status == "OK")
                    {
                        _nav.NavigateTo("DashboardWindow", new { client = pinVm.ClientInfo, playerData }, screenIndex: 0);
                        _nav.NavigateTo("DashboardHome", new { client = pinVm.ClientInfo, playerData });
                    }
                    else
                    {
                        MessageBox.Show("No se pudieron cargar los datos del jugador", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    StatusMessage = "Autenticación cancelada o fallida";
                    await Task.Delay(2000);
                    StartListening();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al procesar tarjeta: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                StartListening();
            }
        }

        private async void OnQRCodeRead(object sender, QRCodeReadEventArgs e)
        {
            Debug.WriteLine("QR recibido: " + e.QRCodeData);
            // Aquí puedes agregar la lógica para procesar el QR
        }

        private async void OnNFCCardDetected(object sender, NFCCardEventArgs e)
        {
            Debug.WriteLine("NFC recibido: " + e.NFCData);
            // Aquí puedes agregar la lógica para procesar el NFC
        }

        private async Task DoLoginAsync()
        {
            try
            {
                // Autenticar usuario con email y password
                var client = await _auth.AuthenticateAsync(Email, Password);
                if (client == null)
                {
                    MessageBox.Show("Credenciales inválidas", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Intentar cargar los datos del jugador
                PlayerDataResponse playerData;
                try
                {
                    playerData = await _playerSvc.GetPlayerDataAsync();
                    if (playerData == null || playerData.Status != "OK")
                    {
                        MessageBox.Show("No se pudieron cargar los datos del jugador", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar los datos del jugador: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Si todo está correcto, navega al dashboard
                _nav.NavigateTo("DashboardWindow", new { client, playerData }, screenIndex: 0);
                _nav.NavigateTo("DashboardHome", new { client, playerData });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al autenticar: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }




        #endregion

        #region Cleanup

        public void Cleanup()
        {
            _magneticCardReader.CardRead -= OnMagneticCardRead;
            _qrCodeReader.CodeRead -= OnQRCodeRead;
            _nfcReader.CardDetected -= OnNFCCardDetected;
            StopListening();
        }

        #endregion
    }
}
