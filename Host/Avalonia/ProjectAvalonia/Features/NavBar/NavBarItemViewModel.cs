using System.Threading.Tasks;
using System.Windows.Input;
using ProjectAvalonia.ViewModels.Navigation;
using ReactiveUI;

namespace ProjectAvalonia.Features.NavBar;

public enum NavBarItemSelectionMode
{
    Selected = 0
    , Button = 1
    , Toggle = 2
}

public abstract class NavBarItemViewModel : RoutableViewModel
{
    private readonly NavigationMode _defaultNavigationMode;
    private bool _isSelected;

    protected NavBarItemViewModel(
        NavigationMode defaultNavigationMode = NavigationMode.Clear
    )
    {
        _defaultNavigationMode = defaultNavigationMode;
        SelectionMode = NavBarItemSelectionMode.Selected;
        OpenCommand = ReactiveCommand.CreateFromTask<bool>(execute: OnOpenCommandExecutedAsync);
    }

    public NavBarItemSelectionMode SelectionMode
    {
        get;
        protected init;
    }

    public bool IsSelectable => SelectionMode == NavBarItemSelectionMode.Selected;

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            switch (SelectionMode)
            {
                case NavBarItemSelectionMode.Selected:
                    this.RaiseAndSetIfChanged(backingField: ref _isSelected, newValue: value);
                    break;

                case NavBarItemSelectionMode.Button:
                case NavBarItemSelectionMode.Toggle:
                    break;
            }
        }
    }

    public ICommand OpenCommand
    {
        get;
    }

    private async Task OnOpenCommandExecutedAsync(
        bool enableReSelection = false
    )
    {
        if (!enableReSelection && IsSelected)
        {
            return;
        }

        IsSelected = true;
        await OnOpen(defaultNavigationMode: _defaultNavigationMode);
    }

    protected virtual Task OnOpen(
        NavigationMode defaultNavigationMode
    )
    {
        if (SelectionMode == NavBarItemSelectionMode.Toggle)
        {
            Toggle();
        }
        else
        {
            Navigate().To(viewmodel: this, mode: defaultNavigationMode);
        }

        return Task.CompletedTask;
    }

    public virtual void Toggle()
    {
    }
}