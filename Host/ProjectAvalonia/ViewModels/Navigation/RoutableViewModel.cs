using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Common.ViewModels;
using ProjectAvalonia.ViewModels.Dialogs;
using ProjectAvalonia.ViewModels.Dialogs.Base;

using ReactiveUI;

namespace ProjectAvalonia.ViewModels.Navigation;

public abstract partial class RoutableViewModel
    : ViewModelBase
        , INavigatable
    , IToolBarHost
{
    private CompositeDisposable? _currentDisposable;
    [AutoNotify] private bool _enableBack;
    [AutoNotify] private bool _enableCancel;
    [AutoNotify] private bool _enableCancelOnEscape;
    [AutoNotify] private bool _enableCancelOnPressed;
    [AutoNotify] private bool _isActive;

    protected RoutableViewModel()
    {
        BackCommand = ReactiveCommand.Create(() => Navigate().Back());
        CancelCommand = ReactiveCommand.Create(() => Navigate().Clear());
    }

    public abstract string Title
    {
        get;
        protected set;
    }

    public abstract string? LocalizedTitle
    {
        get;
        protected set;
    }

    public NavigationTarget CurrentTarget
    {
        get;
        internal set;
    }

    public virtual NavigationTarget DefaultTarget => NavigationTarget.HomeScreen;

    public virtual string IconName
    {
        get;
        protected set;
    } = "navigation_regular";

    public virtual string IconNameFocused
    {
        get;
        protected set;
    } = "navigation_regular";

    public ICommand? NextCommand
    {
        get;
        protected set;
    }

    public ICommand? SkipCommand
    {
        get;
        protected set;
    }

    public ICommand BackCommand
    {
        get;
        protected set;
    }

    public ICommand CancelCommand
    {
        get;
        protected set;
    }

    public abstract MenuViewModel? ToolBar
    {
        get;

    }

    public void OnNavigatedTo(
        bool isInHistory
    ) => DoNavigateTo(isInHistory);

    public void OnNavigatedTo(
        bool isInHistory
        , object Parameter = null
    ) => DoNavigateTo(isInHistory, Parameter);

    void INavigatable.OnNavigatedFrom(
        bool isInHistory
    ) => DoNavigateFrom(isInHistory);

    public void SetTitle(
        string localizedString
    )
    {
        if (!string.IsNullOrWhiteSpace(localizedString))
        {
            Title = localizedString.GetLocalized();
            LocalizedTitle = localizedString.GetLocalized();
        }
    }

    private void DoNavigateTo(
        bool isInHistory
    )
    {
        if (_currentDisposable is not null)
        {
            throw new Exception("Can't navigate to something that has already been navigated to.");
        }

        _currentDisposable = new CompositeDisposable();

        OnNavigatedTo(isInHistory, _currentDisposable);
    }

    private void DoNavigateTo(
        bool isInHistory
        , object? Parameter = null
    )
    {
        if (_currentDisposable is not null)
        {
            throw new Exception("Can't navigate to something that has already been navigated to.");
        }

        _currentDisposable = new CompositeDisposable();

        if (Parameter is null)
        {
            OnNavigatedTo(isInHistory, _currentDisposable);
        }

        else
        {
            OnNavigatedTo(isInHistory, _currentDisposable, Parameter);
        }
    }

    protected virtual void OnNavigatedTo(
        bool isInHistory
        , CompositeDisposable disposables
    )
    {
    }

    protected virtual void OnNavigatedTo(
        bool isInHistory
        , CompositeDisposable disposables
        , object? Parameter = null
    )
    {
    }

    private void DoNavigateFrom(
        bool isInHistory
    )
    {
        OnNavigatedFrom(isInHistory);

        _currentDisposable?.Dispose();
        _currentDisposable = null;
    }

    public INavigationStack<RoutableViewModel> Navigate()
    {
        var currentTarget = CurrentTarget == NavigationTarget.Default ? DefaultTarget : CurrentTarget;

        return Navigate(currentTarget);
    }

    public static INavigationStack<RoutableViewModel> Navigate(
        NavigationTarget currentTarget
    ) =>
        currentTarget switch
        {
            NavigationTarget.HomeScreen => NavigationState.Instance.HomeScreenNavigation
            ,
            NavigationTarget.DialogScreen => NavigationState.Instance.DialogScreenNavigation
            ,
            NavigationTarget.FullScreen => NavigationState.Instance.FullScreenNavigation
            ,
            NavigationTarget.CompactDialogScreen => NavigationState.Instance.CompactDialogScreenNavigation
            ,
            _ => throw new NotSupportedException()
        };

    public void SetActive()
    {
        if (NavigationState.Instance.HomeScreenNavigation.CurrentPage is { } homeScreen)
        {
            homeScreen.IsActive = false;
        }

        if (NavigationState.Instance.DialogScreenNavigation.CurrentPage is { } dialogScreen)
        {
            dialogScreen.IsActive = false;
        }

        if (NavigationState.Instance.FullScreenNavigation.CurrentPage is { } fullScreen)
        {
            fullScreen.IsActive = false;
        }

        if (NavigationState.Instance.CompactDialogScreenNavigation.CurrentPage is { } compactDialogScreen)
        {
            compactDialogScreen.IsActive = false;
        }

        IsActive = true;
    }

    protected virtual void OnNavigatedFrom(
        bool isInHistory
    )
    {
    }

    protected void EnableAutoBusyOn(
        params ICommand[] commands
    )
    {
        foreach (var command in commands)
        {
            (command as IReactiveCommand)?.IsExecuting
                .ObserveOn(RxApp.MainThreadScheduler)
                .Skip(1)
                .ToProperty(this, x => x.IsBusy, out _isBusy);
        }
    }

    public async Task<DialogResult<TResult>> NavigateDialogAsync<TResult>(
        DialogViewModelBase<TResult> dialog
    )
        => await NavigateDialogAsync(dialog, CurrentTarget);

    public static async Task<DialogResult<TResult>> NavigateDialogAsync<TResult>(
        DialogViewModelBase<TResult> dialog
        , NavigationTarget target
        , NavigationMode navigationMode = NavigationMode.Normal
    )
    {
        var dialogTask = dialog.GetDialogResultAsync();

        Navigate(target).To(dialog, navigationMode);

        var result = await dialogTask;

        Navigate(target).Back();

        return result;
    }

    protected async Task ShowErrorAsync(
        string title
        , string message
        , string caption
        , NavigationTarget target = NavigationTarget.Default
    )
    {
        var dialog = new ShowErrorDialogViewModel(message, title, caption);

        var navigationTarget = target != NavigationTarget.Default
            ? target
            : CurrentTarget == NavigationTarget.CompactDialogScreen
                ? NavigationTarget.CompactDialogScreen
                : NavigationTarget.DialogScreen;

        await NavigateDialogAsync(dialog, navigationTarget);
    }

    protected void SetupCancel(
        bool enableCancel
        , bool enableCancelOnEscape
        , bool enableCancelOnPressed
    )
    {
        EnableCancel = enableCancel;
        EnableCancelOnEscape = enableCancelOnEscape;
        EnableCancelOnPressed = enableCancelOnPressed;
    }
}