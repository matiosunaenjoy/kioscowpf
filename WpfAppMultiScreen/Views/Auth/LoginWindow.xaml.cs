using KioscoAutogestion.Baluma.Casino.App.Arguments;
using KioscoAutogestion.Baluma.Casino.App.Services.Navigation;
using KioscoAutogestion.Baluma.Casino.App.Services.Readers;
using KioscoAutogestion.Baluma.Casino.App.ViewModels;
using System;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace KioscoAutogestion.Baluma.Casino.App.Views
{
    public partial class LoginWindow : Window
    {
        private readonly INavigationService _nav;
        private readonly LoginViewModel _vm;
        private readonly IMagneticCardReaderService _magReader;
        private readonly StringBuilder _sb = new StringBuilder();
        private readonly WedgeInputService _wedgeSvc;

        public LoginWindow(
            INavigationService nav,
            LoginViewModel vm,
            IMagneticCardReaderService magneticCardReader,
            IQRCodeReaderService qrSvc)
        {
            InitializeComponent();

            _nav = nav;
            _magReader = magneticCardReader;
            DataContext = _vm = vm;

            // Configurar el servicio de entrada wedge
            _wedgeSvc = new WedgeInputService(_magReader, qrSvc);

            // Maneja la entrada del teclado como si fuera wedge
            this.PreviewTextInput += (s, e) =>
            {
                _wedgeSvc.OnTextInput(e.Text);
                e.Handled = true;
            };

            // Poner el foco en el textbox oculto para capturar la entrada
            Loaded += (_, __) => txtReader.Focus();

            // Arrancar los lectores nada más abrir la ventana
            vm.StartListening();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _vm?.StartListening();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            _vm?.Cleanup();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MostrarModalTarjeta_Click(object sender, RoutedEventArgs e)
        {
            borderModalTarjeta.Width  = panelOpciones.ActualWidth;
            borderModalTarjeta.Height = panelOpciones.ActualHeight;
            modalTarjeta.IsOpen       = true;
        }

        private void CerrarModalTarjeta_Click(object sender, RoutedEventArgs e)
        {
            modalTarjeta.IsOpen = false;
        }

        private void MostrarModalQR_Click(object sender, RoutedEventArgs e)
        {
            borderModalQR.Width  = panelOpciones.ActualWidth;
            borderModalQR.Height = panelOpciones.ActualHeight;
            modalQR.IsOpen       = true;
        }

        private void CerrarModalQR_Click(object sender, RoutedEventArgs e)
        {
            modalQR.IsOpen = false;
        }

        private void MostrarModalNFC_Click(object sender, RoutedEventArgs e)
        {
            borderModalNFC.Width  = panelOpciones.ActualWidth;
            borderModalNFC.Height = panelOpciones.ActualHeight;
            modalNFC.IsOpen       = true;
        }

        private void CerrarModalNFC_Click(object sender, RoutedEventArgs e)
        {
            modalNFC.IsOpen = false;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm)
                vm.Password = ((PasswordBox)sender).Password;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            (DataContext as LoginViewModel)?.Cleanup();
            base.OnClosing(e);
        }
    }
}
