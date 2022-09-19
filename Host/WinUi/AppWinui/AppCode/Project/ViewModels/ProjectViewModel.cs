using AppUsecases.Entities.FileTemplate;
using AppUsecases.Entities;
using System.Collections.ObjectModel;

using AppWinui.AppCode.AppUtils.Contracts.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;

namespace AppWinui.AppCode.Project.ViewModels;
public class ProjectViewModel : ObservableRecipient,INavigationAware
{
    public ExplorerViewViewModel ExplorerViewModel
    {
        get; set;
    }

    public ProjectViewModel()
    {
        ExplorerViewModel = App.GetService<ExplorerViewViewModel>();
    }
    #region InterfaceImplementedMethods
    public void OnNavigatedFrom()
    {

    }
    public void OnNavigatedTo(object parameter)
    {
        if (parameter is string projectPath)
        {
           
        }
    }
    #endregion
}
