using KioscoAutogestion.Baluma.Casino.App.Helpers;
using KioscoAutogestion.Baluma.Casino.App.Models;
using KioscoAutogestion.Baluma.Casino.App.Services.Navigation;
using KioscoAutogestion.Baluma.Casino.App.Services.Player;
using KioscoAutogestion.Baluma.Casino.App.Services.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MessageBox = System.Windows.MessageBox;

namespace KioscoAutogestion.Baluma.Casino.App.ViewModels
{
  
    public class DashboardViewModel : ViewModelBase, IInitializable
    {
        private readonly IPlayerService _playerService;
        private ClientInfo _client;
        private PlayerResponse _player;
        private bool _isLoading;
        private readonly INavigationService _navigation;


        // Campos internos para visibilidad
        private bool _isOptionsVisible = true;
        private bool _isContentVisible = false;

        // Comando síncrono para navegar y alternar visibilidad
        public ICommand NavigateSyncCommand { get; }
        public ICommand BackCommand { get; }

        public override async void Initialize(object parameter)
        {
            if (parameter is ClientInfo info)
            {
                Client = info;            
            }         
            await LoadPlayerDataAsync();
        }
        public DashboardViewModel(IPlayerService playerService, INavigationService navigation)
        {
            _playerService = playerService;
            _navigation = navigation;

            // Inicializa tu comando síncrono
            NavigateSyncCommand = new RelayCommandSync<object>(OnNavigateSync);

            // Comando para volver
            BackCommand = new RelayCommandSync<object>(_ =>
            {
                IsOptionsVisible = true;
                IsContentVisible = false;
            }, _ => IsContentVisible);
        }

        private void OnNavigateSync(object param)
        {
            if (param is string key)
            {
                // 1) Ocultar panel de opciones
                IsOptionsVisible = false;
                OnPropertyChanged(nameof(IsOptionsVisible));

                // 2) Mostrar contenido dinámico
                IsContentVisible = true;
                OnPropertyChanged(nameof(IsContentVisible));

                // ¡Notifica al BackCommand que ahora puede ejecutarse!
                ((RelayCommandSync<object>)BackCommand).NotifyCanExecuteChanged();

                // 3) Navegar
                _navigation.NavigateTo(key);
            }
        }

        // Propiedades enlazables para visibilidad
        // Propiedades que enlazas en XAML
        public bool IsOptionsVisible
        {
            get => _isOptionsVisible;
            private set
            {
                if (_isOptionsVisible == value) return;
                _isOptionsVisible = value;
                OnPropertyChanged(nameof(IsOptionsVisible));
            }
        }

        public bool IsContentVisible
        {
            get => _isContentVisible;
            private set
            {
                if (_isContentVisible == value) return;
                _isContentVisible = value;
                OnPropertyChanged(nameof(IsContentVisible));
                // Notifica también aquí por si cambias visibilidad sin navegar:
                ((RelayCommandSync<object>)BackCommand).NotifyCanExecuteChanged();
            }
        }

        public PlayerResponse Player
        {
            get => _player;
            private set
            {
                _player = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(AccumulatedPoints));
                OnPropertyChanged(nameof(PointsDisplay));

                OnPropertyChanged(nameof(Greeting));
            }
        }

        public ClientInfo Client
        {
            get => _client;
            private set
            {
                _client = value;
                OnPropertyChanged();
            }
        }


        public bool IsLoading
        {
            get => _isLoading;
            private set
            {
                if (_isLoading == value) return;
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
                OnPropertyChanged(nameof(PointsDisplay));
            }
        }
        
        public string Greeting
        {
            get
            {
                if (Player?.Acct != null)
                    return $"¡Bienvenido, cliente {Player.Acct}!";
                if (Client != null)
                    return $"¡Bienvenido, cliente {Client.UserNumber}!";
                return string.Empty;
            }
        }


        public int AccumulatedPoints => Player?.Balances?.PointsBal ?? 0;

        public string PointsDisplay =>
            IsLoading
                ? "Cargando puntos..."
                : $"Puntos acumulados: {AccumulatedPoints}";

        private async Task LoadPlayerDataAsync()
        {
            IsLoading = true;
            try
            {
                var response = await _playerService.GetPlayerDataAsync();
                if (response != null && response.Status == "success")
                {
                    Player = response.Data.Response;
                }
                else
                {
                    MessageBox.Show("No se pudo cargar los datos del jugador.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Error al obtener datos: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
