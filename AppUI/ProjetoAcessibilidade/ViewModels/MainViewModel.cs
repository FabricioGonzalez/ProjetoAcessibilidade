using System.Collections.ObjectModel;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Core.Models;

using ProjetoAcessibilidade.Contracts.Services;
using ProjetoAcessibilidade.Contracts.ViewModels;

using SystemApplication.Services.LastOpenModule.Contracts;

namespace ProjetoAcessibilidade.ViewModels;

public class MainViewModel : ObservableRecipient, INavigationAware
{
    private readonly INavigationService navigationService;
    private readonly ILastOpenService _lastOpenService;

    private ObservableCollection<LastOpenModel> items = new();
    public ObservableCollection<LastOpenModel> Items
    {
        get => items;
        set => SetProperty(ref items, value);
    }


    private ICommand _navigateToProjectCommand;

    public ICommand NavigateToProjectCommand => _navigateToProjectCommand ??= new RelayCommand(OnNavigateToProject);

    public MainViewModel(INavigationService navigationService, ILastOpenService lastOpenService)
    {
        _lastOpenService = lastOpenService;
        this.navigationService = navigationService;
    }
    private void OnNavigateToProject()
    {
        navigationService.NavigateTo(typeof(ProjectViewModel).FullName, "param");
    }

    public async void OnNavigatedTo(object parameter)
    {
        var r = await _lastOpenService.GetLastOpenItems();
        App.MainWindow.DispatcherQueue.TryEnqueue(() =>
        {
            Items = new(r);
        });
    }
    public void OnNavigatedFrom()
    {

    }
}
