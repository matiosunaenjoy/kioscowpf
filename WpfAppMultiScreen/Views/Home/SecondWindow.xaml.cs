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

namespace KioscoAutogestion.Baluma.Casino.App
{
    /// <summary>
    /// Lógica de interacción para SecondWindow.xaml
    /// </summary>
    public partial class SecondWindow : Window
    {

        public SecondWindow()
        {
            InitializeComponent();
            
            // Ruta del video: puede ser una ruta relativa o absoluta.
            // Si el video está en la carpeta del proyecto, asegúrate de que su propiedad "Copy to Output Directory" esté configurada.
            string videoPath = "Videos/Casino.mp4"; // Ejemplo de ruta relativa

            // Asigna la fuente al MediaElement
            videoPlayer.Source = new Uri(videoPath, UriKind.Relative);

            // Inicia la reproducción
            videoPlayer.Play();
        }

        public void PlayVideo(string videoPath)
        {
            videoPlayer.Source = new Uri(videoPath, UriKind.Relative);
            videoPlayer.Position = TimeSpan.Zero;
            videoPlayer.Play();
        }

        private void videoPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            videoPlayer.Position = TimeSpan.Zero;
            videoPlayer.Play();
        }

    }
}
