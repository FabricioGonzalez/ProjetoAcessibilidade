using Microsoft.UI.Xaml;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoAcessibilidade.Settings_Module.Services.Contracts
{
    public interface IThemeSelectorService
    {
        ElementTheme Theme { get; }
        Task InitializeAsync();
        Task SetThemeAsync(ElementTheme theme);
        Task SetRequestedThemeAsync();
    }
}
