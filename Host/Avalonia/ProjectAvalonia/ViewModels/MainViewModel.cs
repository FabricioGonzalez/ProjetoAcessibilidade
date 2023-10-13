using System;
using System.Diagnostics.CodeAnalysis;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

using Avalonia.Controls;

using Common;

using ProjectAvalonia.Common.Helpers;
using ProjectAvalonia.Common.Models;
using ProjectAvalonia.Common.Services;
using ProjectAvalonia.Common.Services.LocationService;
using ProjectAvalonia.Common.Validation;
using ProjectAvalonia.Common.ViewModels;
using ProjectAvalonia.Features.NavBar;
using ProjectAvalonia.Features.PDFViewer.ViewModels;
using ProjectAvalonia.Features.Project.Services;
using ProjectAvalonia.Features.Project.ViewModels;
using ProjectAvalonia.Features.Project.ViewModels.Components;
using ProjectAvalonia.Features.SearchBar;
using ProjectAvalonia.Features.SearchBar.Sources;
using ProjectAvalonia.Features.Settings.ViewModels;
using ProjectAvalonia.Features.TemplateEdit.Services;
using ProjectAvalonia.Features.TemplateEdit.ViewModels;
using ProjectAvalonia.ViewModels.Dialogs.Base;
using ProjectAvalonia.ViewModels.Navigation;

using ReactiveUI;

using XmlDatasource.ExplorerItems;
using XmlDatasource.ProjectItems;
using XmlDatasource.Solution;
using XmlDatasource.ValidationRules;

namespace ProjectAvalonia.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public static readonly Interaction<Exception, Unit> ValidatedErrors = new();
    private readonly CreateSolutionViewModel _createSolution;
    private readonly EditingItemViewModel _editingItemPage;
    [AutoNotify] private DialogScreenViewModel _compactDialogScreen;

    [AutoNotify] private DialogScreenViewModel _dialogScreen;
    [AutoNotify] private DialogScreenViewModel _fullScreen;

    [AutoNotify] private NavBarViewModel _navBar;
    private PreviewerViewModel _previewPrintPage;
    private ProjectViewModel _projectPage;
    private SettingsPageViewModel _settingsPage;
    private TemplateEditViewModel _templatePage;

    [AutoNotify] private string _title = Constants.AppName;

    [AutoNotify] private MenuViewModel? _toolBarMenu;
    [AutoNotify] private WindowState _windowState;

    [AutoNotify] private string _version;

    protected MainViewModel()
    {
        ApplyUiConfigWindowSate();

        ValidatedErrors.RegisterHandler(
            interaction =>
            {
                ErrorMessage = interaction.Input.Message;

                interaction.SetOutput(Unit.Default);
            });

        this.ValidateProperty(
            property: vm => vm.ErrorMessage,
            validateMethod: errors =>
            {
                errors.Add(
                    severity: ErrorSeverity.Warning,
                    error: ErrorMessage ?? "Nothing");
            });

        Version = EnvironmentHelpers.GetExecutableVersion();

        _dialogScreen = new DialogScreenViewModel();
        _fullScreen = new DialogScreenViewModel(NavigationTarget.FullScreen);
        _compactDialogScreen = new DialogScreenViewModel(NavigationTarget.CompactDialogScreen);
        MainScreen = new TargettedNavigationStack(NavigationTarget.HomeScreen);
        NavigationState.Register(
            homeScreenNavigation: MainScreen,
            dialogScreenNavigation: DialogScreen,
            fullScreenNavigation: FullScreen,
            compactDialogScreenNavigation: CompactDialogScreen);

        UiServices.Initialize();
        CreateDI();

        NavigationManager.RegisterType(_navBar);
        RegisterViewModels();

        Observable.FromEventPattern<UpdateStatus>(it => ServicesConfig.UpdateManager.UpdateAvailableToGet += it, it => ServicesConfig.UpdateManager.UpdateAvailableToGet -= it)
          .Subscribe(it =>
          {
              NotificationHelpers.Show(title: "Atualização Disponível", message: "Há uma Atualização disponivel para instalação. \n Clique aqui para instalar!", time: 0, onClick: () =>
              {
                  ServicesConfig.UpdateManager.StartInstallingNewVersion();
              });
          });

        RxApp.MainThreadScheduler.Schedule(async () => await _navBar.InitialiseAsync());

        this.WhenAnyValue(x => x.WindowState)
            .Where(state => state != WindowState.Minimized)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(state => ServicesConfig.UiConfig.WindowState = state.ToString());

        IsMainContentEnabled = this.WhenAnyValue(
                property1: x => x.DialogScreen.IsDialogOpen,
                property2: x => x.FullScreen.IsDialogOpen,
                property3: x => x.CompactDialogScreen.IsDialogOpen,
                selector: (
                    dialogIsOpen
                    , fullScreenIsOpen
                    , compactIsOpen
                ) => !(dialogIsOpen || fullScreenIsOpen || compactIsOpen))
            .ObserveOn(RxApp.MainThreadScheduler);

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
            .ObserveOn(RxApp.MainThreadScheduler)
            .Do(page =>
            {
                page.SetActive();

                ToolBarMenu = MainScreen?.CurrentPage?.ToolBar;
            })
            .Subscribe();

        IsBusy = MainScreen.CurrentPage is { IsBusy: true } ||
                 DialogScreen.CurrentPage is { IsBusy: true } ||
                 FullScreen.CurrentPage is { IsBusy: true } ||
                 CompactDialogScreen.CurrentPage is { IsBusy: true };

        SearchBar = CreateSearchBar();


    }

    public IObservable<bool> IsMainContentEnabled
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

    public bool IsBusy
    {
        get;
    }

    private void CreateDI()
    {
        var xmlExplorerItemsDatasource = new XmlExplorerItemDatasourceImpl();
        var xmlSolutionDatasource = new SolutionDatasourceImpl();
        var xmlProjectItemDatasource = new ProjectItemDatasourceImpl();
        var xmlValidationRulesDatasource = new ValidationRulesDatasourceImpl();
        var locationService = new LocationService();
        var fileDialogService = new FilePickerService();

        var importItemsService = new ImportTemplateService(httpClient: ServicesConfig.UpdateManager.HttpClient,
            filePickerService: fileDialogService);

        var exportItemsService = new ExportTemplateService(filePickerService: fileDialogService);

        var solutionManipulation =
            new SolutionService(datasourceImpl: xmlSolutionDatasource,
            locationService: locationService,
            filePickerService: fileDialogService);

        var itemsService = new ItemsService(explorerItems: xmlExplorerItemsDatasource
            , projectItemDatasource: xmlProjectItemDatasource);

        var editableItemService = new EditableItemService(xmlProjectItemDatasource);

        var validationRulesService = new ValidationRulesService(xmlValidationRulesDatasource);

        var itemValidationViewModel = new ItemValidationViewModel();

        var templateEditTab = new TemplateEditTabViewModel();

        _settingsPage = new SettingsPageViewModel();

        _templatePage = new TemplateEditViewModel(templateEditTab: templateEditTab
            , itemValidationTab: itemValidationViewModel,
            itemsService: itemsService
            , editableItemService: editableItemService,
            validationRulesService: validationRulesService,
            importTemplateService: importItemsService,
            exportTemplateService: exportItemsService,
            filePickerService: fileDialogService);

        _projectPage = new ProjectViewModel(solutionService: solutionManipulation,
            itemsService: itemsService
            , editableItemService: editableItemService,
            validationRulesService: validationRulesService
            , locationService: locationService,
            filePickerService: fileDialogService);

        _previewPrintPage = new PreviewerViewModel();
        _navBar = new NavBarViewModel();
    }

    public void ClearStacks()
    {
        MainScreen.Clear();
        DialogScreen.Clear();
        FullScreen.Clear();
        CompactDialogScreen.Clear();
    }

    public async Task OpenProject(
        string projectPath
    ) =>
        Instance.MainScreen.To(
            viewmodel: _projectPage,
            Parameter: projectPath);

    public async Task PrintProject(
string path
    )
    {
        await OpenProject(projectPath: path);

        await _projectPage.PrintProjectCommand.Execute();

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
        SettingsPageViewModel.Register(_settingsPage);

        TemplateEditViewModel.Register(_templatePage);

        ProjectViewModel.Register(_projectPage);
        PreviewerViewModel.Register(_previewPrintPage);

        GeneralSettingsTabViewModel.RegisterLazy(
            () =>
            {
                _settingsPage.SelectedTab = 0;
                return _settingsPage;
            });

        AdvancedSettingsTabViewModel.RegisterLazy(
            () =>
            {
                _settingsPage.SelectedTab = 2;
                return _settingsPage;
            });

        TemplateEditTabViewModel.RegisterLazy(
            () =>
            {
                /*_templatePage.SelectedTab = 0;*/

                return _templatePage;
            });
    }

    public void ApplyUiConfigWindowSate() => WindowState =
        (WindowState)Enum.Parse(
            enumType: typeof(WindowState),
            value: ServicesConfig.UiConfig.WindowState);

    [SuppressMessage(
        category: "Reliability",
        checkId: "CA2000:Dispose objects before losing scope",
        Justification = "Same lifecycle as the application. Won't be disposed separately.")]
    private SearchBarViewModel CreateSearchBar()
    {
        // This subject is created to solve the circular dependency between the sources and SearchBarViewModel
        var filterChanged = new Subject<string>();

        var source = new CompositeSearchSource(
            new ActionsSearchSource(filterChanged),
            new SettingsSearchSource(
                settingsPage: _settingsPage,
                query: filterChanged));

        var searchBar = new SearchBarViewModel(source.Changes);

        searchBar
            .WhenAnyValue(a => a.SearchText)
            .WhereNotNull()
            .Subscribe(filterChanged);

        return searchBar;
    }
}