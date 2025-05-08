using KioscoAutogestion.Baluma.Casino.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using KioscoAutogestion.Baluma.Casino.App.Helpers;
using MessageBox = System.Windows.MessageBox;
using KioscoAutogestion.Baluma.Casino.App.Services.Auth;
using KioscoAutogestion.Baluma.Casino.App.Services.Navigation;

namespace KioscoAutogestion.Baluma.Casino.App.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        private readonly IAuthenticationService _auth;
        private readonly INavigationService _nav;

        public RegisterViewModel(IAuthenticationService auth, INavigationService nav)
        {
            _auth = auth;
            _nav = nav;
            RegisterCommand = new RelayCommand(async _ => await RegisterAsync(), _ => CanRegister);
        }

        public string ClientNumber { get; set; }
        public string Pin { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }

        public ICommand RegisterCommand { get; }
        private bool CanRegister => !string.IsNullOrEmpty(ClientNumber) && !string.IsNullOrEmpty(Pin);

        private async Task RegisterAsync()
        {
            var success = await _auth.RegisterAsync(ClientNumber, Pin);
            if (success)
            {
                MessageBox.Show("Cliente registrado con éxito", "Registro", MessageBoxButton.OK, MessageBoxImage.Information);
                _nav.NavigateTo("LoginView", null);
            }
            else
            {
                MessageBox.Show("Error al registrar cliente", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
