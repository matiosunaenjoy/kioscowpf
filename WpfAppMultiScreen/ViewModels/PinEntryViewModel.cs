using KioscoAutogestion.Baluma.Casino.App.Helpers;
using KioscoAutogestion.Baluma.Casino.App.Models;
using KioscoAutogestion.Baluma.Casino.App.Services.Auth;
using KioscoAutogestion.Baluma.Casino.App.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace KioscoAutogestion.Baluma.Casino.App.ViewModels
{
    public class PinEntryViewModel : ViewModelBase
    {
        private readonly string _cardData;
        private readonly IAuthenticationService _authService;
        private readonly INavigationService _nav;
        private string _currentPin = "";
        private string _displayPin = "";
        private string _errorMessage;

        public string DisplayPin
        {
            get => _displayPin;
            private set
            {
                if (_displayPin == value) return;
                _displayPin = value;
                OnPropertyChanged();
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                if (_errorMessage == value) return;
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public bool IsAuthenticated { get; private set; }
        public ClientInfo ClientInfo { get; private set; }

        public ICommand NumberCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand EnterCommand { get; }

        public PinEntryViewModel(string cardData, IAuthenticationService authService,INavigationService nav)
        {
            _cardData = cardData;
            _authService = authService;
            _nav = nav;

            // Usar las nuevas clases sincrónicas para los métodos void
            NumberCommand = new RelayCommandSync<string>(AddDigitToPin);
            DeleteCommand = new RelayCommandSync(_ => RemoveLastDigit());

            // Mantener RelayCommand para el método async
            //EnterCommand = new RelayCommand(_ => VerifyPinAsync());
            EnterCommand = new RelayCommand(async _ => await pruebaLogin());
        }

        private void AddDigitToPin(string digit)
        {
            // Limitar el PIN a 6 dígitos (ajustar según necesidades)
            if (_currentPin.Length < 6)
            {
                _currentPin += digit;
                UpdatePinDisplay();
            }
        }

        private void RemoveLastDigit()
        {
            if (_currentPin.Length > 0)
            {
                _currentPin = _currentPin.Substring(0, _currentPin.Length - 1);
                UpdatePinDisplay();
            }
        }

        private void UpdatePinDisplay()
        {
            // Mostrar asteriscos en lugar del PIN real
            DisplayPin = new string('*', _currentPin.Length);
        }

        private async Task pruebaLogin()
        {
            var client = await _authService.AuthenticateAsync("rrodriguez@enjoy.cl", "123456");
            if (client != null)
            {
                // 1) Abro el DashboardWindow y paso el ClientInfo al VM
                //_nav.NavigateTo("DashboardWindow", client, screenIndex: 0);
                // 2) Cargo la vista home dentro del dashboard
                //_nav.NavigateTo("DashboardHome", client);

                //prueba
                _nav.NavigateTo("MainDashboard", client, screenIndex: 0);
                //_nav.NavigateTo("DashboardHome", client);
            }
            else
            {
                System.Windows.MessageBox.Show(
                    "Credenciales inválidas",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private async Task VerifyPinAsync()
        {
            try
            {
                // Verificar PIN con el servicio de autenticación
                ClientInfo = await _authService.AuthenticateAsync(_cardData, _currentPin);



                if (ClientInfo != null)
                {
                    IsAuthenticated = true;

                    // Cerrar el diálogo con resultado exitoso
                    CloseDialogWithResult(true);
                }
                else
                {
                    ErrorMessage = "PIN incorrecto. Intente nuevamente por favor.";
                    _currentPin = "";
                    UpdatePinDisplay();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error al verificar PIN: " + ex.Message;
                _currentPin = "";
                UpdatePinDisplay();
            }
        }

        // Este método será asignado por DialogService
        public Action<bool?> CloseDialogWithResult { get; set; }
    }
}
