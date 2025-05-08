using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioscoAutogestion.Baluma.Casino.App.Services.Utils
{
    public interface IDialogService
    {
        Task<bool?> ShowDialogAsync<T>(object viewModel) where T : System.Windows.Window;
    }
}
