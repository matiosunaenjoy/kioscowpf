// App.xaml.cs
using KioscoAutogestion.Baluma.Casino.App.Helpers;
using KioscoAutogestion.Baluma.Casino.App.Services.Auth;
using KioscoAutogestion.Baluma.Casino.App.Services.Navigation;
using KioscoAutogestion.Baluma.Casino.App.Services.Player;
using KioscoAutogestion.Baluma.Casino.App.Services.Readers;
using KioscoAutogestion.Baluma.Casino.App.Services.Utils;
using KioscoAutogestion.Baluma.Casino.App.ViewModels;
using KioscoAutogestion.Baluma.Casino.App.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;
using System.Windows.Forms;
using Application = System.Windows.Application;

namespace KioscoAutogestion.Baluma.Casino.App
{
    public partial class App : Application
    {
        private IServiceProvider _container;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
           
            var services = new ServiceCollection();

            // 1) Servicios de infraestructura
            services.AddSingleton<ISessionService, SessionService>();
            services.AddSingleton<INavigationService, NavigationService>();

            // 2) Nuestro handler que inyecta el Bearer token a cada request
            services.AddTransient<AuthHeaderHandler>();

            // 3) Typed HttpClient para Auth (no inyecta token)
            services
              .AddHttpClient<IAuthenticationService, AuthenticationService>(client =>
              {
                  client.BaseAddress = new Uri("https://svc.baluma.com.uy/");
                  client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
              });

            // 4) Typed HttpClient para Player (sí inyecta token)
            //services
            //  .AddHttpClient<IPlayerService, PlayerService>(client =>
            //  {
            //      client.BaseAddress = new Uri("https://svc.baluma.com.uy/");
            //      client.DefaultRequestHeaders.Accept.Add(
            //        new MediaTypeWithQualityHeaderValue("application/json"));
            //  })
            //  .AddHttpMessageHandler<AuthHeaderHandler>();

            // 5) En su lugar, registra el FakePlayerService:
            services.AddSingleton<IPlayerService, FakePlayerService>();

            // 5) Mock o reales para otros servicios
            // ... cualquier otro
            // App.xaml.cs o donde configuras tus servicios
            services.AddSingleton<IMagneticCardReaderService, MagneticCardReaderService>();
            services.AddSingleton<IQRCodeReaderService, QRCodeReaderService>();
            services.AddSingleton<INFCReaderService, NFCReaderService>();
            services.AddSingleton<IDialogService, DialogService>();

            // 6) ViewModels
            //services.AddTransient<LoginViewModel>();
            services.AddTransient<LoginViewModel>(sp =>
                new LoginViewModel(
                    sp.GetRequiredService<IAuthenticationService>(),
                    sp.GetRequiredService<IPlayerService>(),       // ahora lo inyectas
                    sp.GetRequiredService<INavigationService>(),
                    sp.GetRequiredService<IMagneticCardReaderService>(),
                    sp.GetRequiredService<IQRCodeReaderService>(),
                    sp.GetRequiredService<INFCReaderService>(),
                    sp.GetRequiredService<IDialogService>()
                ));

            services.AddTransient<RegisterViewModel>();
            services.AddTransient<DashboardViewModel>();

            // 7) Views, inyectando ViewModels y Navigation
            services.AddTransient<ModernLogin>(sp =>
                new ModernLogin(sp.GetRequiredService<INavigationService>(), sp.GetRequiredService<LoginViewModel>()));
            services.AddTransient<LoginWindow>(sp =>
                new LoginWindow(
                    sp.GetRequiredService<INavigationService>(),
                    sp.GetRequiredService<LoginViewModel>(),
                    sp.GetRequiredService<IMagneticCardReaderService>(), sp.GetRequiredService<IQRCodeReaderService>()));
            services.AddTransient<RegisterWindow>(sp =>
                new RegisterWindow(
                    sp.GetRequiredService<INavigationService>(),
                    sp.GetRequiredService<RegisterViewModel>()));
            services.AddTransient<DashboardWindow>(sp =>
                new DashboardWindow(
                    sp.GetRequiredService<DashboardViewModel>(),
                    sp.GetRequiredService<INavigationService>()));
            // en lugar de AddTransient<DashboardWindow>
            services.AddTransient<MainDashboard>(sp =>
                new MainDashboard(
                    sp.GetRequiredService<DashboardViewModel>(),
                    sp.GetRequiredService<INavigationService>()));


            // 8) Construyo el contenedor
            _container = services.BuildServiceProvider();

            // 9) Configuro rutas de navegación
            var nav = _container.GetRequiredService<INavigationService>();
            nav.Configure("ModernLogin", () => _container.GetRequiredService<ModernLogin>());
            nav.Configure("LoginWindow", () => _container.GetRequiredService<LoginWindow>());
            nav.Configure("RegisterWindow", () => _container.GetRequiredService<RegisterWindow>());
            nav.Configure("DashboardWindow", () => _container.GetRequiredService<DashboardWindow>());
            nav.Configure("MainDashboard", () => _container.GetRequiredService<MainDashboard>());
            // y tus UserControls...
            nav.Configure("DashboardHome", () => new DashboardHome());
            nav.Configure("DashboardPoints", () => new DashboardPoints());
            nav.Configure("DashboardPrizes", () => new DashboardPrizes());
            nav.Configure("DashboardRoute", () => new DashboardRoute());
            nav.Configure("DashboardBenefits", () => new DashboardBenefits());

            // 10) Arranco la primera ventana
            nav.NavigateTo("ModernLogin", screenIndex: 0);
        }
    }
}