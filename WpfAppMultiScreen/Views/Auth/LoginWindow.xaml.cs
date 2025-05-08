using KioscoAutogestion.Baluma.Casino.App.Arguments;
using KioscoAutogestion.Baluma.Casino.App.Services.Navigation;
using KioscoAutogestion.Baluma.Casino.App.Services.Readers;
using KioscoAutogestion.Baluma.Casino.App.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Formats.Tar;
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

namespace KioscoAutogestion.Baluma.Casino.App.Views
{
    public partial class LoginWindow : Window
    {
        private readonly INavigationService _nav;
        private readonly LoginViewModel _vm;
        private readonly IMagneticCardReaderService _magReader;
        private readonly StringBuilder _sb = new StringBuilder();
        private readonly WedgeInputService _wedgeSvc;


        public LoginWindow(INavigationService nav, LoginViewModel vm, IMagneticCardReaderService magneticCardReader, IQRCodeReaderService qrSvc)
        {
            InitializeComponent();
            _nav = nav;
            _magReader = magneticCardReader;
            DataContext = _vm = vm;

            _magReader.CardRead += OnCardRead;

            // Configurar el servicio de entrada wedge
            _wedgeSvc = new WedgeInputService(_magReader, qrSvc);

            // Maneja la entrada del teclado
            this.PreviewTextInput += (s, e) =>
            {
                _wedgeSvc.OnTextInput(e.Text);
                e.Handled = true;
            };

            // Foco en txtReader al cargar la ventana
            Loaded += (_, __) => txtReader.Focus();
            vm.StartListening();
        }


        private void Window_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = true;

            if (e.Text == "\r" || e.Text == "\n")
            {
                var raw = _sb.ToString().Trim();
                _sb.Clear();

                if (_wedgeSvc.IsMagneticFormat(raw))
                {
                    // ➜ Tarjeta magnética detectada
                    _magReader.InjectCardData(raw);
                    Dispatcher.Invoke(() =>
                    {
                        MostrarModalTarjeta_Click(this, null); // Abre el modal del PIN
                    });
                }
                else
                {
                    // ➜ QR detectado
                    _wedgeSvc.OnTextInput(raw);
                }
            }
            else
            {
                _sb.Append(e.Text);
            }
        }



        // Manejo del evento CardRead
        private void OnCardRead(object sender, CardReadEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                // Abre el modal del PIN
                MostrarModalTarjeta_Click(this, null);
            });
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Iniciar escucha automáticamente al cargar la ventana
            _vm?.StartListening();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            // Limpiar recursos al cerrar
            _vm?.Cleanup();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MostrarModalTarjeta_Click(object sender, RoutedEventArgs e)
        {
            borderModalTarjeta.Width = panelOpciones.ActualWidth;
            borderModalTarjeta.Height = panelOpciones.ActualHeight;
            modalTarjeta.IsOpen = true;
        }

        private void CerrarModalTarjeta_Click(object sender, RoutedEventArgs e)
        {
           
            modalTarjeta.IsOpen = false;
        }

        private void MostrarModalQR_Click(object sender, RoutedEventArgs e)
        {
            borderModalQR.Width = panelOpciones.ActualWidth;
            borderModalQR.Height = panelOpciones.ActualHeight;
            modalQR.IsOpen = true;
        }

        private void CerrarModalQR_Click(object sender, RoutedEventArgs e)
        {
            modalQR.IsOpen = false;
        }

        private void MostrarModalNFC_Click(object sender, RoutedEventArgs e)
        {
            borderModalNFC.Width = panelOpciones.ActualWidth;
            borderModalNFC.Height = panelOpciones.ActualHeight;
            modalNFC.IsOpen = true;
        }

        private void CerrarModalNFC_Click(object sender, RoutedEventArgs e)
        {
            modalNFC.IsOpen = false;
        }

        //private void OnLoginConfirmed(object sender, RoutedEventArgs e)
        //{
        //    // Al validarse...
        //    _nav.NavigateTo("DashboardWindow", screenIndex: 0);
        //}
        

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
