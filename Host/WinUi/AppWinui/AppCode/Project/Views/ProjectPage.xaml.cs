using AppWinui.AppCode.AppUtils.Behaviors;
using AppWinui.AppCode.Project.ViewModels;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AppWinui.AppCode.Project.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class ProjectPage : Page
{
    private ProjectViewModel ViewModel
    {
        get; set;
    }

    public ProjectPage()
    {
        InitializeComponent();
        DataContext = App.GetService<ProjectViewModel>();
        ViewModel = (ProjectViewModel)DataContext;
    }
}
