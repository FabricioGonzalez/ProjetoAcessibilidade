using System.Threading.Tasks;
using ProjectAvalonia.ViewModels.Dialogs.Base;

namespace ProjectAvalonia.ViewModels.Navigation;

public static class NavigationExtensions
{
    public static async Task<DialogResult<T>> NavigateDialogAsync<T>(
        this TargettedNavigationStack stack
        , DialogViewModelBase<T> dialog
    )
    {
        stack.To(viewmodel: dialog);

        var result = await dialog.GetDialogResultAsync();

        stack.Back();

        return result;
    }
}

public class TargettedNavigationStack : NavigationStack<RoutableViewModel>
{
    private readonly NavigationTarget _target;

    public TargettedNavigationStack(
        NavigationTarget target
    )
    {
        _target = target;
    }

    public override void Clear()
    {
        if (_target == NavigationTarget.HomeScreen)
        {
            base.Clear(keepRoot: true);
        }
        else
        {
            base.Clear();
        }
    }

    protected override void OnPopped(
        RoutableViewModel page
    )
    {
        base.OnPopped(page: page);

        page.CurrentTarget = NavigationTarget.Default;
    }

    protected override void OnNavigated(
        RoutableViewModel? oldPage
        , bool oldInStack
        , RoutableViewModel? newPage
        , bool newInStack
    )
    {
        base.OnNavigated(oldPage: oldPage, oldInStack: oldInStack, newPage: newPage, newInStack: newInStack);

        if (oldPage is not null && oldPage != newPage)
        {
            oldPage.IsActive = false;
        }

        if (newPage is not null)
        {
            newPage.CurrentTarget = _target;
        }
    }
}