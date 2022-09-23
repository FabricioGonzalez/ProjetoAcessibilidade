using AppWinui.AppCode.AppUtils.Contracts.ViewModels;
using AppWinui.AppCode.AppUtils.ViewModels;
using AppWinui.AppCode.Project.States;

using CommunityToolkit.Mvvm.ComponentModel;

using ReactiveUI;

namespace AppWinui.AppCode.Project.ViewModels;
public class ProjectViewModel : ReactiveObject
{
    public ApplicationViewModel AppViewModel
    {
        get; set;
    }
    public ExplorerViewViewModel ExplorerViewModel
    {
        get; set;
    }
    public RecentOpenedViewModel RecentOpenedViewModel
    {
        get;set;
    }
    public ReportDataState? Report
    {
        get; set;
    }
   
    private bool _isProjectOpened = false;
    public bool IsProjectOpened
    {
        get => _isProjectOpened;
        set => this.RaiseAndSetIfChanged(
            ref _isProjectOpened,
            value,
            nameof(IsProjectOpened));
    }

    public ProjectViewModel()
    {
        ExplorerViewModel = App.GetService<ExplorerViewViewModel>();
        AppViewModel = App.GetService<ApplicationViewModel>();
    }
}
