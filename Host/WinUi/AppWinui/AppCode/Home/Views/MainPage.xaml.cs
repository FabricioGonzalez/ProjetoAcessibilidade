using AppWinui.AppCode.AppUtils.Behaviors;
using AppWinui.AppCode.Home.ViewModels;

using Microsoft.UI.Xaml;
using AppWinui.AppCode.Project.Views;

using Microsoft.UI.Xaml.Controls;

namespace AppWinui.AppCode.Home.Views;

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
