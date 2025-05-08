using KioscoAutogestion.Baluma.Casino.App.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KioscoAutogestion.Baluma.Casino.App.Services.Utils
{
    public class DialogService : IDialogService
    {
        

        public Task<bool?> ShowDialogAsync<T>(object viewModel)
        where T : System.Windows.Window
        {
            // Recupero la ventana activa para asignarla como Owner
            var owner = System.Windows.Application.Current
                                   .Windows
                                   .OfType<Window>()
                                   .FirstOrDefault(w => w.IsActive);

            bool? dialogResult = null;

            // Me aseguro de ejecutar la creación y ShowDialog en el hilo de UI
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                // Creo la ventana T
                var window = (T)Activator.CreateInstance(typeof(T));

                // Le asigno Owner y DataContext
                window.Owner = owner;
                window.DataContext = viewModel;

                // Muestro modal y recupero el resultado
                dialogResult = window.ShowDialog();
            });

            // Devuelvo el resultado envuelto en un Task
            return Task.FromResult(dialogResult);
        }
    }
}
