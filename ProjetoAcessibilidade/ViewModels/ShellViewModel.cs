using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Text.Json;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Navigation;

using Projeto.Core.Models;

using ProjetoAcessibilidade.Contracts.Services;
using ProjetoAcessibilidade.Services;

namespace ProjetoAcessibilidade.ViewModels;

public class ShellViewModel : ObservableRecipient
{
    private bool _isBackEnabled;
    private object _selected;
    private ICommand _menuFileExitCommand;
    private ICommand _menuFileNewCommand;
    private ICommand _menuFileOpenCommand;
    private ICommand _menuSettingsCommand;
    private ICommand _menuViewsMainCommand;

    public ICommand MenuFileExitCommand => _menuFileExitCommand ??= new RelayCommand(OnMenuFileExit);
    public ICommand MenuFileNewCommand => _menuFileNewCommand ??= new AsyncRelayCommand(OnMenuFileNew);
    public ICommand MenuFileOpenCommand => _menuFileOpenCommand ??= new AsyncRelayCommand(OnMenuFileOpen);
    public ICommand MenuSettingsCommand => _menuSettingsCommand ??= new RelayCommand(OnMenuSettings);
    public ICommand MenuViewsMainCommand => _menuViewsMainCommand ??= new RelayCommand(OnMenuViewsMain);

    public INavigationService NavigationService
    {
        get;
    }

    public bool IsBackEnabled
    {
        get => _isBackEnabled;
        set => SetProperty(ref _isBackEnabled, value);
    }

    public object Selected
    {
        get => _selected;
        set => SetProperty(ref _selected, value);
    }

    private readonly IFileSelectorService fileSelectorService;

    public ShellViewModel(INavigationService navigationService, IFileSelectorService pickerService)
    {
        NavigationService = navigationService;
        NavigationService.Navigated += OnNavigated;
        fileSelectorService = pickerService;
    }

    private void OnNavigated(object sender, NavigationEventArgs e) => IsBackEnabled = NavigationService.CanGoBack;

    private async Task OnMenuFileOpen()
    {
        var result = await fileSelectorService.OpenFile(new string[] { ".prja" });

        if (result is not null)
        {
            var folder = await result.GetParentAsync();

            var solution = new ProjectSolutionModel()
            {
                FileName = result.Name,
                FilePath = result.Path,
                ParentFolderName = folder.Name,
                ParentFolderPath = folder.Path,
            };


            var data = await result.OpenStreamForReadAsync();

            var resultData = await JsonSerializer.DeserializeAsync<ReportDataModel>(data);

            solution.reportData = resultData;
            NavigationService.NavigateTo(typeof(ProjectViewModel).FullName, solution);

        }
    }
    private async Task OnMenuFileNew()
    {
        var result = await fileSelectorService.SaveFile("Arquivo de Projeto", new string[] { ".prja" }, "");

        if (result is not null)
        {
            var folder = await result.GetParentAsync();

            var solution = new ProjectSolutionModel()
            {
                FileName = result.Name,
                FilePath = result.Path,
                ParentFolderName = folder.Name,
                ParentFolderPath = folder.Path,
            };


            var data = await result.OpenStreamForReadAsync();

            var resultData = await JsonSerializer.DeserializeAsync<ReportDataModel>(data);

            solution.reportData = resultData;

            NavigationService.NavigateTo(typeof(ProjectViewModel).FullName, solution);

        }
    }
    private void OnMenuFileExit() => Application.Current.Exit();

    private void OnMenuSettings() => NavigationService.NavigateTo(typeof(SettingsViewModel).FullName);

    private void OnMenuViewsMain() => NavigationService.NavigateTo(typeof(MainViewModel).FullName);
}
