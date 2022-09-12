using System.Collections.ObjectModel;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Core.Models;

using ProjetoAcessibilidade.Contracts.Services;
using ProjetoAcessibilidade.Contracts.ViewModels;
using ProjetoAcessibilidade.ViewModels;

using ReactiveUI;

using SystemApplication.Services.LastOpenModule.Contracts;

namespace ProjetoAcessibilidade.Modules.Home.ViewModels;

public class MainViewModel : ReactiveObject, INavigationAware
{
    private readonly INavigationService navigationService;
    private readonly ILastOpenService _lastOpenService;

    private ObservableCollection<LastOpenModel> items = new();
    public ObservableCollection<LastOpenModel> Items
    {
        get => items;
        set => this.RaiseAndSetIfChanged(ref items, value);
    }

    public MainViewModel(INavigationService navigationService, ILastOpenService lastOpenService)
    {
        _lastOpenService = lastOpenService;
        this.navigationService = navigationService;
    }

    public void OnNavigatedTo(object parameter) { }
    public void OnNavigatedFrom()
    {
    }

    //public async void OnNavigatedTo(object parameter)
    //{
    //    var r = await _lastOpenService.GetLastOpenItems();
    //    App.MainWindow.DispatcherQueue.TryEnqueue(() =>
    //    {
    //        Items = new(r);
    //    });
    //}
    //public void OnNavigatedFrom()
    //{

    //}
}
