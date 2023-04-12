using System;
using System.Collections.Specialized;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;

namespace ProjectAvalonia.Behaviors;

public class NavBarSelectedIndicatorChildBehavior : AttachedToVisualTreeBehavior<Control>
{
    public static readonly AttachedProperty<Control> NavBarItemParentProperty =
        AvaloniaProperty.RegisterAttached<NavBarSelectedIndicatorChildBehavior, Control, Control>(
            name: "NavBarItemParent");

    public static Control GetNavBarItemParent(
        Control element
    ) => element.GetValue(property: NavBarItemParentProperty);

    public static void SetNavBarItemParent(
        Control element
        , Control value
    ) => element.SetValue(property: NavBarItemParentProperty, value: value);

    private void OnLoaded(
        CompositeDisposable disposable
    )
    {
        if (AssociatedObject is null)
        {
            return;
        }

        var sharedState = NavBarSelectedIndicatorParentBehavior.GetParentState(element: AssociatedObject);
        if (sharedState is null)
        {
            Detach();
            return;
        }

        var parent = GetNavBarItemParent(element: AssociatedObject);

        Observable.FromEventPattern<NotifyCollectionChangedEventArgs>(target: parent.Classes
                , eventName: "CollectionChanged")
            .Select(selector: _ => parent.Classes)
            .Select(selector: x => x.Contains(item: ":selected")
                                   && !x.Contains(item: ":pressed")
                                   && !x.Contains(item: ":dragging")
                                   && x.Contains(item: ":selectable"))
            .DistinctUntilChanged()
            .Where(predicate: x => x)
            .ObserveOn(scheduler: AvaloniaScheduler.Instance)
            .Subscribe(onNext: _ => sharedState.AnimateIndicatorAsync(next: AssociatedObject))
            .DisposeWith(compositeDisposable: disposable);

        AssociatedObject.Opacity = 0;

        if (parent.Classes.Contains(item: ":selected"))
        {
            sharedState.SetActive(initial: AssociatedObject);
        }
    }

    protected override void OnAttachedToVisualTree(
        CompositeDisposable disposable
    ) => Dispatcher.UIThread.Post(action: () => OnLoaded(disposable: disposable), priority: DispatcherPriority.Loaded);
}