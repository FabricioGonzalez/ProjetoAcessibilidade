using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using ProjectAvalonia.Common.Models;

namespace ProjectAvalonia.Behaviors;

public class NavBarSelectedIndicatorParentBehavior : AttachedToVisualTreeBehavior<Control>
{
    public static readonly AttachedProperty<NavBarSelectedIndicatorState> ParentStateProperty =
        AvaloniaProperty
            .RegisterAttached<NavBarSelectedIndicatorParentBehavior, Control, NavBarSelectedIndicatorState>(
                name: "ParentState", inherits: true);

    public static NavBarSelectedIndicatorState? GetParentState(
        Control element
    ) => element.GetValue(property: ParentStateProperty);

    public static void SetParentState(
        Control element
        , NavBarSelectedIndicatorState value
    ) => element.SetValue(property: ParentStateProperty, value: value);

    protected override void OnAttachedToVisualTree(
        CompositeDisposable disposable
    )
    {
        if (AssociatedObject is null)
        {
            return;
        }

        var sharedState = new NavBarSelectedIndicatorState();
        SetParentState(element: AssociatedObject, value: sharedState);
        disposable.Add(item: sharedState);
    }
}