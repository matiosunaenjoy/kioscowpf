using KioscoAutogestion.Baluma.Casino.App.ViewModels;
using KioscoAutogestion.Baluma.Casino.App.Views;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using Application = System.Windows.Application;

namespace KioscoAutogestion.Baluma.Casino.App.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        //private readonly Dictionary<string, Func<object>> _factories = new();
        private readonly Dictionary<string, Func<object>> _factories
        = new Dictionary<string, Func<object>>();
        private ContentControl _region;
        private Window _currentWindow;


        public void Configure(string key, Func<object> creator)
        {
            _factories[key] = creator;
        }

        public void InitializeRegion(ContentControl region)
        {
            _region = region;
        }

        public void NavigateTo(string key, object parameter = null, int? screenIndex = null)
        {
            if (!_factories.TryGetValue(key, out var factory))
                throw new InvalidOperationException($"No registrado «{key}».");

            var view = factory();

            // 1) SI ES WINDOW, lo abrimos (cierra anterior)
            if (view is Window w)
            {
                if (w.DataContext is ViewModelBase vmw && parameter != null)
                    vmw.Initialize(parameter);

                if (screenIndex.HasValue)
                {
                    var scr = Screen.AllScreens[screenIndex.Value].WorkingArea;
                    w.WindowStartupLocation = WindowStartupLocation.Manual;
                    w.Left = scr.Left;
                    w.Top = scr.Top;
                    w.Width = scr.Width;
                    w.Height = scr.Height;
                }
                // Mostrar primero la nueva ventana
                w.Show();

                //// Luego cerrar la anterior
                _currentWindow?.Close();
                _currentWindow = w;
                return;
            }
            // 2) SI ES USERCONTROL, inyectarlo
            if (view is System.Windows.Controls.UserControl uc)
            {
                if (_region == null)
                    throw new InvalidOperationException("La región no ha sido inicializada.");

                if (uc.DataContext is ViewModelBase vm && parameter != null)
                    vm.Initialize(parameter);

                _region.Content = uc;
                return;
            }

            throw new InvalidOperationException(
                $"El factory de «{key}» devolvió un {view.GetType().Name} no soportado.");
        }

        
    }
}
