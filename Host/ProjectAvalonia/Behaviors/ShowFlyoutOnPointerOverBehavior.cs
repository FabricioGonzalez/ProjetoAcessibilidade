using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using ReactiveUI;

namespace ProjectAvalonia.Behaviors;

public class ShowFlyoutOnPointerOverBehavior : DisposingBehavior<Control>
{
    protected override void OnAttached(
        CompositeDisposable disposables
    )
    {
        if (AssociatedObject is null)
        {
            return;
        }

        Observable
            .FromEventPattern(target: AssociatedObject, eventName: nameof(AssociatedObject.PointerMoved))
            .Throttle(dueTime: TimeSpan.FromMilliseconds(value: 100))
            .ObserveOn(scheduler: RxApp.MainThreadScheduler)
            .Subscribe(onNext: _ => OnPointerMove())
            .DisposeWith(compositeDisposable: disposables);
    }

    private void OnPointerMove()
    {
        if (AssociatedObject is { } obj && obj.IsPointerOver)
        {
            FlyoutBase.ShowAttachedFlyout(flyoutOwner: AssociatedObject);
        }
    }
}