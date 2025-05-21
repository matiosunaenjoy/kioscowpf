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
using System.Collections.Generic;
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
        private readonly INavigationService _nav;
        private readonly IAuthenticationService _auth;
        private readonly IPlayerService _playerSvc;
        private readonly IMagneticCardReaderService _magReader;
        private readonly IQRCodeReaderService _qrReader;
        private readonly INFCReaderService _nfcReader;
        private readonly IDialogService _dialog;
        private readonly ISessionService _session;

        // Lectura on/off
        private readonly RelayCommandSync _startListeningCmd;
        private readonly RelayCommandSync _stopListeningCmd;

        public ICommand StartListeningCommand => _startListeningCmd;
        public ICommand StopListeningCommand => _stopListeningCmd;

        // Navegación entre ventanas
        public ICommand OpenLoginWindowCommand { get; }
        public ICommand OpenRegisterWindowCommand { get; }

        // Login manual (email/password)
        public ICommand LoginCommand { get; }

        private bool _isListening;
        public bool IsListening
        {
            get => _isListening;
            private set
            {
                if (_isListening == value) return;
                _isListening = value;
                OnPropertyChanged();
                _startListeningCmd.NotifyCanExecuteChanged();
                _stopListeningCmd.NotifyCanExecuteChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private string _statusMessage;
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

        private string _email;
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

        private string _password;
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

        private bool CanLogin =>
            !string.IsNullOrWhiteSpace(Email) &&
            !string.IsNullOrWhiteSpace(Password);

        public LoginViewModel(
            IAuthenticationService auth,
            IPlayerService playerSvc,
            INavigationService nav,
            IMagneticCardReaderService magReader,
            IQRCodeReaderService qrReader,
            INFCReaderService nfcReader,
            IDialogService dialog,
            ISessionService session)
        {
            _auth = auth;
            _playerSvc = playerSvc;
            _nav = nav;
            _magReader = magReader;
            _qrReader = qrReader;
            _nfcReader = nfcReader;
            _dialog = dialog;
            _session = session;

            // 1) Comandos Start/Stop de lectores
            _startListeningCmd = new RelayCommandSync(_ => StartListening(), _ => !IsListening);
            _stopListeningCmd = new RelayCommandSync(_ => StopListening(), _ => IsListening);

            // 2) Comandos de navegación
            OpenLoginWindowCommand = new RelayCommandSync(_ => _nav.NavigateTo("LoginWindow", screenIndex: 0), _ => true);
            OpenRegisterWindowCommand = new RelayCommandSync(_ => _nav.NavigateTo("RegisterWindow", screenIndex: 0), _ => true);

            // 3) Comando de login tradicional
            LoginCommand = new RelayCommand(async _ => await DoLoginAsync(), _ => CanLogin);

            // 4) Suscripción a eventos de lectura
            _magReader.CardRead += OnCardRead;
            _qrReader.CodeRead += OnQrRead;
            _nfcReader.CardDetected += (s, e) => {
                /* implementa NFC si lo necesitas */
            };

            StatusMessage = "Listo para iniciar la lectura";
        }

        #region Lectores

        public void StartListening()
        {
            if (IsListening) return;
            IsListening = true;
            StatusMessage = "Esperando tarjeta o QR...";
            _magReader.StartReading();
            _qrReader.StartReading();
            _nfcReader.StartReading();
        }

        public void StopListening()
        {
            if (!IsListening) return;
            _magReader.StopReading();
            _qrReader.StopReading();
            _nfcReader.StopReading();
            IsListening = false;
            StatusMessage = "Lectores desactivados";
        }

        private async void OnCardRead(object sender, CardReadEventArgs e)
        {
            if (!IsListening) return;
            await HandleCardAsync(e.CardData);
        }

        private async void OnQrRead(object sender, QRCodeReadEventArgs e)
        {
            if (!IsListening) return;
            await HandleQrAsync(e.QRCodeData);
        }

        #endregion

        #region Flujos de autenticación

        private async Task HandleCardAsync(string rawData)
        {
            StopListening();
            try
            {
                // abre diálogo PIN
                var pinVm = new PinEntryViewModel(rawData, _auth, _nav);
                bool? ok = await _dialog.ShowDialogAsync<PinEntryView>(pinVm);
                if (ok != true || !pinVm.IsAuthenticated)
                {
                    StatusMessage = "PIN inválido";
                    await Task.Delay(1500);
                    return;
                }

                // autentica con card+PIN
                var client = await _auth.AuthenticateWithCardAsync(rawData, pinVm.DisplayPin);
                if (client == null)
                {
                    MessageBox.Show("Error al autenticar tarjeta", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                _session.CurrentClient = client;

                // trae datos del jugador
                var player = await _playerSvc.GetPlayerDataAsync();
                if (player == null)
                {
                    //MessageBox.Show("No se pudieron cargar datos del jugador", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // navega al MainDashboard
                Application.Current.Dispatcher.Invoke(() =>
                    _nav.NavigateTo("MainDashboard", new { client, player }, screenIndex: 0)
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error procesando tarjeta: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (IsListening) StopListening();
            }
        }

        private async Task HandleQrAsync(string qrData)
        {
            StopListening();
            try
            {
                var token = qrData.Trim();
                if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                    token = token.Substring(7);

                var client = new ClientInfo
                {
                    Token = token,
                    RefreshToken = token,
                    UserNumber = 0,
                    Permissions = new List<int>(),
                    ForceChangePassword = false
                };
                _session.CurrentClient = client;

                var player = await _playerSvc.GetPlayerDataAsync();
                if (player == null)
                {
                    MessageBox.Show("No se pudieron cargar datos del jugador", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Application.Current.Dispatcher.Invoke(() =>
                    _nav.NavigateTo("MainDashboard", new { client, player }, screenIndex: 0)
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error procesando QR: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (IsListening) StopListening();
            }
        }

        private async Task DoLoginAsync()
        {
            StopListening();
            try
            {
                var client = await _auth.AuthenticateAsync(Email, Password);
                if (client == null)
                {
                    MessageBox.Show("Credenciales inválidas", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                _session.CurrentClient = client;

                var player = await _playerSvc.GetPlayerDataAsync();
                if (player == null)
                {
                    MessageBox.Show("No se pudieron cargar datos del jugador", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    _nav.NavigateTo("DashboardWindow", new { client, player }, screenIndex: 0);
                    _nav.NavigateTo("DashboardHome", new { client, player });
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error autenticando: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (IsListening) StopListening();
            }
        }

        #endregion

        // Después:
        public void Cleanup()
        {
            _magReader.CardRead -= OnCardRead;
            _qrReader.CodeRead -= OnQrRead;
            _nfcReader.CardDetected -= null; // o tu handler real
            StopListening();
        }

    }
}
