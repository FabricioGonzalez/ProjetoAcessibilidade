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
{
    private CompositeDisposable? _currentDisposable;
    [AutoNotify] private bool _enableBack;
    [AutoNotify] private bool _enableCancel;
    [AutoNotify] private bool _enableCancelOnEscape;
    [AutoNotify] private bool _enableCancelOnPressed;
    [AutoNotify] private bool _isActive;

    protected RoutableViewModel()
    {
        BackCommand = ReactiveCommand.Create(execute: () => Navigate().Back());
        CancelCommand = ReactiveCommand.Create(execute: () => Navigate().Clear());
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

    public void OnNavigatedTo(
        bool isInHistory
    ) => DoNavigateTo(isInHistory: isInHistory);

    public void OnNavigatedTo(
        bool isInHistory
        , object Parameter = null
    ) => DoNavigateTo(isInHistory: isInHistory, Parameter: Parameter);

    void INavigatable.OnNavigatedFrom(
        bool isInHistory
    ) => DoNavigateFrom(isInHistory: isInHistory);

    public void SetTitle(string localizedString)
    {
        if (!string.IsNullOrWhiteSpace(value: localizedString))
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
            throw new Exception(message: "Can't navigate to something that has already been navigated to.");
        }

        _currentDisposable = new CompositeDisposable();

        OnNavigatedTo(isInHistory: isInHistory, disposables: _currentDisposable);
    }

    private void DoNavigateTo(
        bool isInHistory
        , object? Parameter = null
    )
    {
        if (_currentDisposable is not null)
        {
            throw new Exception(message: "Can't navigate to something that has already been navigated to.");
        }

        _currentDisposable = new CompositeDisposable();

        if (Parameter is null)
        {
            OnNavigatedTo(isInHistory: isInHistory, disposables: _currentDisposable);
        }

        else
        {
            OnNavigatedTo(isInHistory: isInHistory, disposables: _currentDisposable, Parameter: Parameter);
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
        OnNavigatedFrom(isInHistory: isInHistory);

        _currentDisposable?.Dispose();
        _currentDisposable = null;
    }

    public INavigationStack<RoutableViewModel> Navigate()
    {
        var currentTarget = CurrentTarget == NavigationTarget.Default ? DefaultTarget : CurrentTarget;

        return Navigate(currentTarget: currentTarget);
    }

    public static INavigationStack<RoutableViewModel> Navigate(
        NavigationTarget currentTarget
    ) =>
        currentTarget switch
        {
            NavigationTarget.HomeScreen => NavigationState.Instance.HomeScreenNavigation,
            NavigationTarget.DialogScreen => NavigationState.Instance.DialogScreenNavigation,
            NavigationTarget.FullScreen => NavigationState.Instance.FullScreenNavigation,
            NavigationTarget.CompactDialogScreen => NavigationState.Instance.CompactDialogScreenNavigation,
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
                .ObserveOn(scheduler: RxApp.MainThreadScheduler)
                .Skip(count: 1)
                .ToProperty(source: this, property: x => x.IsBusy, result: out _isBusy);
        }
    }

    public async Task<DialogResult<TResult>> NavigateDialogAsync<TResult>(
        DialogViewModelBase<TResult> dialog
    )
        => await NavigateDialogAsync(dialog: dialog, target: CurrentTarget);

    public static async Task<DialogResult<TResult>> NavigateDialogAsync<TResult>(
        DialogViewModelBase<TResult> dialog
        , NavigationTarget target
        , NavigationMode navigationMode = NavigationMode.Normal
    )
    {
        var dialogTask = dialog.GetDialogResultAsync();

        Navigate(currentTarget: target).To(viewmodel: dialog, mode: navigationMode);

        var result = await dialogTask;

        Navigate(currentTarget: target).Back();

        return result;
    }

    protected async Task ShowErrorAsync(
        string title
        , string message
        , string caption
        , NavigationTarget target = NavigationTarget.Default
    )
    {
        var dialog = new ShowErrorDialogViewModel(message: message, title: title, caption: caption);

        var navigationTarget = target != NavigationTarget.Default
            ? target
            : CurrentTarget == NavigationTarget.CompactDialogScreen
                ? NavigationTarget.CompactDialogScreen
                : NavigationTarget.DialogScreen;

        await NavigateDialogAsync(dialog: dialog, target: navigationTarget);
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