using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using ProjetoAcessibilidade.Contracts.Services;

namespace ProjetoAcessibilidade.ViewModels;

public class MainViewModel : ObservableRecipient
{
    private readonly INavigationService navigationService;
    private ICommand _navigateToProjectCommand;

    public ICommand NavigateToProjectCommand => _navigateToProjectCommand ??= new RelayCommand(OnNavigateToProject);

    public MainViewModel(INavigationService navigationService)
    {
        this.navigationService = navigationService;
    }
    private void OnNavigateToProject()
    {
        navigationService.NavigateTo(typeof(ProjectViewModel).FullName,"param");
    }
}
