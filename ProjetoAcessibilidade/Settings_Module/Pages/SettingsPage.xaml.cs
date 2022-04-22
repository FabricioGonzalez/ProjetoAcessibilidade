using CommunityToolkit.Mvvm.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

using ProjetoAcessibilidade.Settings_Module.ViewModels;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ProjetoAcessibilidade.Settings_Module.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public SettingsViewModel ViewModel { get; set; }
        public SettingsPage()
        {
            ViewModel = Ioc.Default.GetService<SettingsViewModel>();
                                                                         
            DataContext = ViewModel;
            this.InitializeComponent();
        }
    }
}
