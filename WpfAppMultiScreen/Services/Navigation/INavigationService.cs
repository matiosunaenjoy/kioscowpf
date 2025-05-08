// INavigationService.cs
using System;
using System.Windows.Controls;

namespace KioscoAutogestion.Baluma.Casino.App.Services.Navigation
{
    public interface INavigationService
    {
        void Configure(string key, Func<object> creator);
        void NavigateTo(string key, object parameter = null, int? screenIndex = null);
        void InitializeRegion(ContentControl region);
    }
}
