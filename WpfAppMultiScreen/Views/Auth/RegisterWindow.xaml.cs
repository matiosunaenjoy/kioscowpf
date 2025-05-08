using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using KioscoAutogestion.Baluma.Casino.App.Services.Navigation;
using KioscoAutogestion.Baluma.Casino.App.ViewModels;

namespace KioscoAutogestion.Baluma.Casino.App.Views
{
    /// <summary>
    /// Lógica de interacción para RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private readonly INavigationService _nav;
        private readonly RegisterViewModel _vm;

        public RegisterWindow(INavigationService nav, RegisterViewModel vm)
        {
            InitializeComponent();
            _nav = nav;
            DataContext = _vm = vm;
        }
        private void OnRegisterConfirmed(object sender, RoutedEventArgs e)
        {
            // Tras registrarse...
            _nav.NavigateTo("DashboardWindow", screenIndex: 0);
        }
        //public RegisterWindow()
        //{
        //    InitializeComponent();
        //}
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            //var pruebas = new Prueba();        
            //pruebas.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //pruebas.ShowDialog();
            //this.Close();
            // Aquí pones la lógica para registrar al usuario...
            //System.Windows.MessageBox.Show("¡Cuenta creada!", "Registro", MessageBoxButton.OK, MessageBoxImage.Information);
            //this.Close();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
