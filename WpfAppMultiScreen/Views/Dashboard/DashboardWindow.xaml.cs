using KioscoAutogestion.Baluma.Casino.App.Models;
using KioscoAutogestion.Baluma.Casino.App.Services.Navigation;
using KioscoAutogestion.Baluma.Casino.App.ViewModels;
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
using Application = System.Windows.Application;
using UserControl = System.Windows.Controls.UserControl;


namespace KioscoAutogestion.Baluma.Casino.App.Views
{

    public partial class DashboardWindow : Window
    {
        private readonly INavigationService _nav;
        private readonly DashboardViewModel _vm;

        public DashboardWindow(DashboardViewModel vm, INavigationService nav)
        {
            InitializeComponent();
            DataContext = vm;
            _vm = vm;
            _nav = nav;
            _nav.InitializeRegion(MainContentRegion);
            //_nav.NavigateTo("DashboardHome");
        }

     


        // Handlers de click para el menú lateral:
        private void NavigateToPointsExchange(object sender, MouseButtonEventArgs e)
            => _nav.NavigateTo("DashboardPoints", _vm.Client);

        private void NavigateToEnjoyRoute(object sender, MouseButtonEventArgs e)
            => _nav.NavigateTo("DashboardRoute", _vm.Client);

        private void NavigateToPrizesExchange(object sender, MouseButtonEventArgs e)
            => _nav.NavigateTo("DashboardPrizes", _vm.Client);

        private void NavigateToBenefits(object sender, MouseButtonEventArgs e)
            => _nav.NavigateTo("DashboardBenefits", _vm.Client);

        private void ExitApplication(object sender, MouseButtonEventArgs e)
            => Application.Current.Shutdown();

        
    }
}
