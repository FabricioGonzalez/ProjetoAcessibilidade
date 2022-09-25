using System.Reactive.Disposables;

using AppUsecases.Entities;

using AppWinui.AppCode.AppUtils.ViewModels;
using AppWinui.AppCode.Project.DTOs;
using AppWinui.AppCode.Project.ViewModels;

using Common;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using ReactiveUI;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AppWinui.AppCode.Project.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class ProjectPage : Page,IViewFor<ProjectViewModel>
{
    private ApplicationViewModel AppViewModel
    {
        get; set;
    }
    /// <summary>The ViewModel to display</summary>


    public ProjectViewModel ViewModel
    {
        get
        {
            return (ProjectViewModel)GetValue(ViewModelProperty);
        }
        set
        {
            SetValue(ViewModelProperty, value);
        }
    }

    // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ViewModelProperty =
        DependencyProperty.Register("ViewModel", 
            typeof(ProjectViewModel),
            typeof(ProjectPage), 
            new PropertyMetadata(0));

    object IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (ProjectViewModel)value;
    }

    public ProjectPage()
    {
        InitializeComponent();
        DataContext = App.GetService<ProjectViewModel>();
        ViewModel = (ProjectViewModel)DataContext;
        AppViewModel = App.GetService<ApplicationViewModel>();

        this.WhenActivated(d =>
        {
            this.BindCommand(ViewModel,
           vm => vm.OpenProjectCommand,
           v => v.openMenuOption)
            .DisposeWith(d);
        });

       
    }
}
