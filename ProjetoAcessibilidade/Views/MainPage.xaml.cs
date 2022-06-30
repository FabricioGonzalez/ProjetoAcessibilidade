using Microsoft.UI.Xaml.Controls;

using ProjetoAcessibilidade.ViewModels;

namespace ProjetoAcessibilidade.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
    }
}
