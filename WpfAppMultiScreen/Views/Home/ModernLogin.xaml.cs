using KioscoAutogestion.Baluma.Casino.App.Services.Navigation;
using KioscoAutogestion.Baluma.Casino.App.ViewModels;
using KioscoAutogestion.Baluma.Casino.App.Views;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Application = System.Windows.Application;
using Button = System.Windows.Controls.Button;

namespace KioscoAutogestion.Baluma.Casino.App
{
    public partial class ModernLogin : Window
    {

        private readonly INavigationService _nav;
        private readonly LoginViewModel _vm;
        public ModernLogin(INavigationService nav, LoginViewModel vm)
        {
            InitializeComponent();
            _nav = nav;
            DataContext = _vm = vm;
        }

       
    }
}