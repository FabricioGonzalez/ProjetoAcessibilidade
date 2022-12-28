using System;
using System.Collections.Generic;
using System.Linq;

<<<<<<< HEAD:ProjetoAcessibilidade/Services/PageService.cs

using Microsoft.UI.Xaml.Controls;

using ProjetoAcessibilidade.Contracts.Services;
using ProjetoAcessibilidade.Modules.Home.ViewModels;
using ProjetoAcessibilidade.Modules.Main.ViewModels;
using ProjetoAcessibilidade.Modules.Print.ViewModels;
using ProjetoAcessibilidade.Modules.TemplateEditing.ViewModels;
using ProjetoAcessibilidade.ViewModels;
using ProjetoAcessibilidade.Views;
=======
using Microsoft.UI.Xaml.Controls;

using ProjectWinUI.Src.Navigation.Contracts;
>>>>>>> reinit:Host/WinUi/ProjectWinUI/Src/Navigation/Services/PageService.cs

using ReactiveUI;

namespace ProjectWinUI.Src.Navigation.Services;
public class PageService : IPageService
{
    private static readonly Dictionary<string, Type> _pages = new();

    public Type GetPageType(string key)
    {
        Type? pageType;
        lock (_pages)
        {
            if (!_pages.TryGetValue(key, out pageType))
            {
                throw new ArgumentException($"Page not found: {key}. Did you forget to call PageService.Configure?");
            }
        }

        return pageType;
    }

<<<<<<< HEAD:ProjetoAcessibilidade/Services/PageService.cs
    private void Configure<VM, V>()
=======
    public static void Configure<VM, V>()
        where VM : ReactiveObject
>>>>>>> reinit:Host/WinUi/ProjectWinUI/Src/Navigation/Services/PageService.cs
        where V : Page
    {
        lock (_pages)
        {
            var key = typeof(VM).FullName!;
            if (_pages.ContainsKey(key))
            {
                throw new ArgumentException($"The key {key} is already configured in PageService");
            }

            var type = typeof(V);
            if (_pages.Any(p => p.Value == type))
            {
                throw new ArgumentException($"This type is already configured with key {_pages.First(p => p.Value == type).Key}");
            }

            _pages.Add(key, type);
        }
    }
}

