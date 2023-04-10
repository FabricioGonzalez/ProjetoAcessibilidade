using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using ProjectAvalonia.ViewModels.Navigation;
using ReactiveUI;

namespace ProjectAvalonia.ViewModels.Dialogs.Base;

public partial class DialogScreenViewModel : TargettedNavigationStack
{
    [AutoNotify] private bool _isDialogOpen;

    public DialogScreenViewModel(
        NavigationTarget navigationTarget = NavigationTarget.DialogScreen
    ) : base(target: navigationTarget)
    {
        this.WhenAnyValue(property1: x => x.IsDialogOpen)
            .Skip(count: 1) // Skip the initial value change (which is false).
            .DistinctUntilChanged()
            .Subscribe(
                onNext: x =>
                {
                    if (!x)
                    {
                        CloseScreen();
                    }
                });
    }

    protected override void OnNavigated(
        RoutableViewModel? oldPage
        , bool oldInStack
        , RoutableViewModel? newPage
        , bool newInStack
    )
    {
        base.OnNavigated(oldPage: oldPage, oldInStack: oldInStack, newPage: newPage, newInStack: newInStack);

        IsDialogOpen = CurrentPage is not null;
    }

    private static void CloseDialogs(
        IEnumerable<RoutableViewModel> navigationStack
    )
    {
        // Close all dialogs so the awaited tasks can complete.
        // - DialogViewModelBase.ShowDialogAsync()
        // - DialogViewModelBase.GetDialogResultAsync()

        foreach (var routable in navigationStack)
        {
            if (routable is DialogViewModelBase dialog)
            {
                dialog.IsDialogOpen = false;
            }
        }
    }

    private void CloseScreen()
    {
        var navStack = Stack.ToList();
        Clear();

        CloseDialogs(navigationStack: navStack);
    }
}