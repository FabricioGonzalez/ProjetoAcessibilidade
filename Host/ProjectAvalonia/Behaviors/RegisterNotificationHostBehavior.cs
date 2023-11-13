using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Controls;
using ProjectAvalonia.Common.Helpers;
using ReactiveUI;

namespace ProjectAvalonia.Behaviors;

public class RegisterNotificationHostBehavior : DisposingBehavior<Window>
{
    protected override void OnAttached(
        CompositeDisposable disposables
    )
    {
        if (AssociatedObject is null)
        {
            return;
        }

        NotificationHelpers.SetNotificationManager(host: AssociatedObject);

        // Must set notification host again after theme changing.
        Observable
            .FromEventPattern(target: AssociatedObject, eventName: nameof(AssociatedObject.ResourcesChanged))
            .ObserveOn(scheduler: RxApp.MainThreadScheduler)
            .Subscribe(onNext: _ => NotificationHelpers.SetNotificationManager(host: AssociatedObject))
            .DisposeWith(compositeDisposable: disposables);
    }
}