using System;
using System.Diagnostics.CodeAnalysis;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Avalonia.Controls;
using Common;
using ProjectAvalonia.Features.NavBar;
using ProjectAvalonia.Features.PDFViewer.ViewModels;
using ProjectAvalonia.Features.Project.ViewModels;
using ProjectAvalonia.Features.Project.ViewModels.Components;
using ProjectAvalonia.Features.SearchBar;
using ProjectAvalonia.Features.SearchBar.SearchBar.Sources;
using ProjectAvalonia.Features.SearchBar.Sources;
using ProjectAvalonia.Features.Settings.ViewModels;
using ProjectAvalonia.Features.TemplateEdit.ViewModels;
using ProjectAvalonia.Stores;
using ProjectAvalonia.ViewModels.Dialogs.Base;
using ProjectAvalonia.ViewModels.Navigation;
using ReactiveUI;

namespace ProjectAvalonia.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly CreateSolutionViewModel _createSolution;
    private readonly EditingItemViewModel _editingItemPage;
    private readonly PreviewerViewModel _previewPrintPage;
    private readonly ProjectViewModel _projectPage;
    private readonly SettingsPageViewModel _settingsPage;
    private readonly TemplateEditViewModel _templatePage;
    [AutoNotify] private DialogScreenViewModel _compactDialogScreen;

    [AutoNotify] private DialogScreenViewModel _dialogScreen;
    [AutoNotify] private DialogScreenViewModel _fullScreen;

    [AutoNotify] private NavBarViewModel _navBar;

    /*[AutoNotify] private StatusIconViewModel _statusIcon;*/
    [AutoNotify] private string _title = Constants.AppName;
    [AutoNotify] private WindowState _windowState;


    public MainViewModel()
    {
        ApplyUiConfigWindowSate();

        _dialogScreen = new DialogScreenViewModel();
        _fullScreen = new DialogScreenViewModel(navigationTarget: NavigationTarget.FullScreen);
        _compactDialogScreen = new DialogScreenViewModel(navigationTarget: NavigationTarget.CompactDialogScreen);
        MainScreen = new TargettedNavigationStack(target: NavigationTarget.HomeScreen);
        NavigationState.Register(homeScreenNavigation: MainScreen,
            dialogScreenNavigation: DialogScreen,
            fullScreenNavigation: FullScreen,
            compactDialogScreenNavigation: CompactDialogScreen);

        UiServices.Initialize();

        _settingsPage = new SettingsPageViewModel();
        _templatePage = new TemplateEditViewModel();
        _editingItemPage = new EditingItemViewModel();
        _projectPage = new ProjectViewModel();
        _previewPrintPage = new PreviewerViewModel();
        _navBar = new NavBarViewModel();

        NavigationManager.RegisterType(instance: _navBar);
        RegisterViewModels();

        RxApp.MainThreadScheduler.Schedule(action: async () => await _navBar.InitialiseAsync());

        this.WhenAnyValue(property1: x => x.WindowState)
            .Where(predicate: state => state != WindowState.Minimized)
            .ObserveOn(scheduler: RxApp.MainThreadScheduler)
            .Subscribe(onNext: state => ServicesConfig.UiConfig.WindowState = state.ToString());

        IsMainContentEnabled = this.WhenAnyValue(
                property1: x => x.DialogScreen.IsDialogOpen,
                property2: x => x.FullScreen.IsDialogOpen,
                property3: x => x.CompactDialogScreen.IsDialogOpen,
                selector: (
                    dialogIsOpen
                    , fullScreenIsOpen
                    , compactIsOpen
                ) => !(dialogIsOpen || fullScreenIsOpen || compactIsOpen))
            .ObserveOn(scheduler: RxApp.MainThreadScheduler);

        this.WhenAnyValue(
                property1: x => x.DialogScreen.CurrentPage,
                property2: x => x.CompactDialogScreen.CurrentPage,
                property3: x => x.FullScreen.CurrentPage,
                property4: x => x.MainScreen.CurrentPage,
                selector: (
                    dialog
                    , compactDialog
                    , fullScreenDialog
                    , mainScreen
                ) => compactDialog ?? dialog ?? fullScreenDialog ?? mainScreen)
            .WhereNotNull()
            .ObserveOn(scheduler: RxApp.MainThreadScheduler)
            .Do(onNext: page => page.SetActive())
            .Subscribe();

        SearchBar = CreateSearchBar();
    }

    public IObservable<bool> IsMainContentEnabled
    {
        get;
    }

    public string NetworkBadgeName
    {
        get;
    }

    public TargettedNavigationStack MainScreen
    {
        get;
    }

    public SearchBarViewModel SearchBar
    {
        get;
    }

    public static MainViewModel Instance
    {
        get;
    } = new();

    public bool IsBusy =>
        MainScreen.CurrentPage is { IsBusy: true } ||
        DialogScreen.CurrentPage is { IsBusy: true } ||
        FullScreen.CurrentPage is { IsBusy: true } ||
        CompactDialogScreen.CurrentPage is { IsBusy: true };

    public void ClearStacks()
    {
        MainScreen.Clear();
        DialogScreen.Clear();
        FullScreen.Clear();
        CompactDialogScreen.Clear();
    }

    public void OpenProject(
        string projectPath
    ) =>
        Instance.MainScreen.To(viewmodel: _projectPage, Parameter: projectPath);

    public void PrintProject(
        SolutionState solutionState
    )
    {
        Instance.FullScreen.To(viewmodel: _previewPrintPage, Parameter: solutionState);

        OpenProject(projectPath: solutionState.FilePath);
    }

    public void Initialize()
    {
        /*   StatusIcon.Initialize();*/

        /* if (Services.Config.Network != Network.Main)
         {
             Title += $" - {Services.Config.Network}";
         }*/
    }

    private void RegisterViewModels()
    {
        SettingsPageViewModel.Register(createInstance: _settingsPage);

        TemplateEditViewModel.Register(createInstance: _templatePage);

        ProjectViewModel.Register(createInstance: _projectPage);
        PreviewerViewModel.Register(createInstance: _previewPrintPage);

        GeneralSettingsTabViewModel.RegisterLazy(createInstance: () =>
        {
            _settingsPage.SelectedTab = 0;
            return _settingsPage;
        });

        AdvancedSettingsTabViewModel.RegisterLazy(createInstance: () =>
        {
            _settingsPage.SelectedTab = 2;
            return _settingsPage;
        });

        TemplateEditTabViewModel.RegisterLazy(createInstance: () =>
        {
            _templatePage.SelectedTab = 0;

            return _templatePage;
        });
    }

    public void ApplyUiConfigWindowSate() => WindowState =
        (WindowState)Enum.Parse(enumType: typeof(WindowState), value: ServicesConfig.UiConfig.WindowState);

    [SuppressMessage(category: "Reliability", checkId: "CA2000:Dispose objects before losing scope"
        , Justification = "Same lifecycle as the application. Won't be disposed separately.")]
    private SearchBarViewModel CreateSearchBar()
    {
        // This subject is created to solve the circular dependency between the sources and SearchBarViewModel
        var filterChanged = new Subject<string>();

        var source = new CompositeSearchSource(
            new ActionsSearchSource(query: filterChanged),
            new SettingsSearchSource(settingsPage: _settingsPage, query: filterChanged));

        var searchBar = new SearchBarViewModel(itemsObservable: source.Changes);

        searchBar
            .WhenAnyValue(property1: a => a.SearchText)
            .WhereNotNull()
            .Subscribe(observer: filterChanged);

        return searchBar;
    }
}