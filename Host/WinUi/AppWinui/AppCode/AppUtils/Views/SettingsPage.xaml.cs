using AppWinui.AppCode.AppUtils.Behaviors;
using AppWinui.AppCode.AppUtils.ViewModels;
using AppWinui.AppCode.Project.Views;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace AppWinui.AppCode.AppUtils.Views;

// TODO: Set the URL for your privacy policy by updating SettingsPage_PrivacyTermsLink.NavigateUri in Resources.resw.
public sealed partial class SettingsPage : Page
{
    public SettingsViewModel ViewModel
    {
        get;
    }
   
    public SettingsPage()
    {
        ViewModel = App.GetService<SettingsViewModel>();
        InitializeComponent();
    }
}
