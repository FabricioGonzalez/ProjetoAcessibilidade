using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;

using Avalonia.Controls;

using Common;

using ProjectAvalonia.Features.NavBar;
using ProjectAvalonia.Features.Project.ViewModels;
using ProjectAvalonia.Features.SearchBar;
using ProjectAvalonia.Features.SearchBar.Sources;
using ProjectAvalonia.Features.Settings.ViewModels;
using ProjectAvalonia.Features.TemplateEdit.ViewModels;
using ProjectAvalonia.ViewModels.Dialogs.Base;
using ProjectAvalonia.ViewModels.Navigation;

using ReactiveUI;

namespace ProjectAvalonia.ViewModels;
public partial class MainViewModel : ViewModelBase
{
    private readonly SettingsPageViewModel _settingsPage;
    private readonly TemplateEditViewModel _templatePage;
    private readonly ProjectViewModel _projectPage;
    [AutoNotify] private DialogScreenViewModel _dialogScreen;
    [AutoNotify] private DialogScreenViewModel _fullScreen;
    [AutoNotify] private DialogScreenViewModel _compactDialogScreen;
    [AutoNotify] private NavBarViewModel _navBar;
    /*[AutoNotify] private StatusIconViewModel _statusIcon;*/
    [AutoNotify] private string _title = Constants.AppName;
    [AutoNotify] private WindowState _windowState;


    public MainViewModel()
    {
        ApplyUiConfigWindowSate();

        _dialogScreen = new DialogScreenViewModel();
        _fullScreen = new DialogScreenViewModel(NavigationTarget.FullScreen);
        _compactDialogScreen = new DialogScreenViewModel(NavigationTarget.CompactDialogScreen);
        MainScreen = new TargettedNavigationStack(NavigationTarget.HomeScreen);
        NavigationState.Register(MainScreen,
            DialogScreen,
            FullScreen,
            CompactDialogScreen);

        UiServices.Initialize();

        _settingsPage = new SettingsPageViewModel();
        _templatePage = new TemplateEditViewModel();
        _projectPage = new ProjectViewModel();
        _navBar = new NavBarViewModel();

        NavigationManager.RegisterType(_navBar);
        RegisterViewModels();

        RxApp.MainThreadScheduler.Schedule(async () => await _navBar.InitialiseAsync());

        this.WhenAnyValue(x => x.WindowState)
            .Where(state => state != WindowState.Minimized)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(state => ServicesConfig.UiConfig.WindowState = state.ToString());

        IsMainContentEnabled = this.WhenAnyValue(
                x => x.DialogScreen.IsDialogOpen,
                x => x.FullScreen.IsDialogOpen,
                x => x.CompactDialogScreen.IsDialogOpen,
                (dialogIsOpen, fullScreenIsOpen, compactIsOpen) => !(dialogIsOpen || fullScreenIsOpen || compactIsOpen))
            .ObserveOn(RxApp.MainThreadScheduler);

        this.WhenAnyValue(
                x => x.DialogScreen.CurrentPage,
                x => x.CompactDialogScreen.CurrentPage,
                x => x.FullScreen.CurrentPage,
                x => x.MainScreen.CurrentPage,
                (dialog, compactDialog, fullScreenDialog, mainScreen) => compactDialog ?? dialog ?? fullScreenDialog ?? mainScreen)
            .WhereNotNull()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Do(page => page.SetActive())
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

    public static MainViewModel Instance { get; } = new();

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

        GeneralSettingsTabViewModel.RegisterLazy(() =>
        {
            _settingsPage.SelectedTab = 0;
            return _settingsPage;
        });

        AdvancedSettingsTabViewModel.RegisterLazy(() =>
        {
            _settingsPage.SelectedTab = 2;
            return _settingsPage;
        });

        TemplateEditTabViewModel.RegisterLazy(() =>
        {
            _templatePage.SelectedTab = 0;

            return _templatePage;
        });


        /*
                AboutViewModel.RegisterLazy(() => new AboutViewModel());
                BroadcasterViewModel.RegisterLazy(() => new BroadcasterViewModel());
                LegalDocumentsViewModel.RegisterLazy(() => new LegalDocumentsViewModel());
                UserSupportViewModel.RegisterLazy(() => new UserSupportViewModel());
                BugReportLinkViewModel.RegisterLazy(() => new BugReportLinkViewModel());
                DocsLinkViewModel.RegisterLazy(() => new DocsLinkViewModel());
                OpenDataFolderViewModel.RegisterLazy(() => new OpenDataFolderViewModel());
                OpenWalletsFolderViewModel.RegisterLazy(() => new OpenWalletsFolderViewModel());
                OpenLogsViewModel.RegisterLazy(() => new OpenLogsViewModel());
                OpenTorLogsViewModel.RegisterLazy(() => new OpenTorLogsViewModel());
                OpenConfigFileViewModel.RegisterLazy(() => new OpenConfigFileViewModel());

                WalletCoinsViewModel.RegisterLazy(() =>
                {
                    if (UiServices.WalletManager.TryGetSelectedAndLoggedInWalletViewModel(out var walletViewModel))
                    {
                        return new WalletCoinsViewModel(walletViewModel);
                    }

                    return null;
                });

         CoinJoinSettingsViewModel.RegisterLazy(() =>
          {
              if (UiServices.WalletManager.TryGetSelectedAndLoggedInWalletViewModel(out var walletViewModel) && !walletViewModel.IsWatchOnly)
              {
                  return walletViewModel.CoinJoinSettings;
              }

              return null;
          });

          WalletSettingsViewModel.RegisterLazy(() =>
          {
              if (UiServices.WalletManager.TryGetSelectedAndLoggedInWalletViewModel(out var walletViewModel))
              {
                  return walletViewModel.Settings;
              }

              return null;
          });

          WalletStatsViewModel.RegisterLazy(() =>
          {
              if (UiServices.WalletManager.TryGetSelectedAndLoggedInWalletViewModel(out var walletViewModel))
              {
                  return new WalletStatsViewModel(walletViewModel);
              }

              return null;
          });

          WalletInfoViewModel.RegisterAsyncLazy(() =>
          {
              if (UiServices.WalletManager.TryGetSelectedAndLoggedInWalletViewModel(out var walletViewModel))
              {
                  async Task<RoutableViewModel?> AuthorizeWalletInfo()
                  {
                      if (!string.IsNullOrEmpty(walletViewModel.Wallet.Kitchen.SaltSoup()))
                      {
                          var pwAuthDialog = new PasswordAuthDialogViewModel(walletViewModel.Wallet);
                          var dialogResult = await RoutableViewModel.NavigateDialogAsync(pwAuthDialog, NavigationTarget.CompactDialogScreen);

                          if (!dialogResult.Result)
                          {
                              return null;
                          }
                      }

                      return new WalletInfoViewModel(walletViewModel);
                  }

                  return AuthorizeWalletInfo();
              }

              Task<RoutableViewModel?> NoWalletInfo() => Task.FromResult<RoutableViewModel?>(null);

              return NoWalletInfo();
          });
  
          SendViewModel.RegisterLazy(() =>
           {
               if (UiServices.WalletManager.TryGetSelectedAndLoggedInWalletViewModel(out var walletViewModel))
               {
                   // TODO: Check if we can send?
                   return new SendViewModel(walletViewModel);
               }

               return null;
           });

           ReceiveViewModel.RegisterLazy(() =>
           {
               if (UiServices.WalletManager.TryGetSelectedAndLoggedInWalletViewModel(out var walletViewModel))
               {
                   return new ReceiveViewModel(walletViewModel.Wallet);
               }

               return null;
           });*/
    }

    public void ApplyUiConfigWindowSate()
    {

        WindowState = (WindowState)Enum.Parse(typeof(WindowState), ServicesConfig.UiConfig.WindowState);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Same lifecycle as the application. Won't be disposed separately.")]
    private SearchBarViewModel CreateSearchBar()
    {
        // This subject is created to solve the circular dependency between the sources and SearchBarViewModel
        var filterChanged = new Subject<string>();

        var source = new CompositeSearchSource(
            new Features.SearchBar.SearchBar.Sources.ActionsSearchSource(filterChanged),
            new SettingsSearchSource(_settingsPage, filterChanged));

        var searchBar = new SearchBarViewModel(source.Changes);

        searchBar
            .WhenAnyValue(a => a.SearchText)
            .WhereNotNull()
            .Subscribe(filterChanged);

        return searchBar;
    }
}

