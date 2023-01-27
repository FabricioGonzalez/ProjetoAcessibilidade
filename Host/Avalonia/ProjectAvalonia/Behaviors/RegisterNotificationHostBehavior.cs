using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

using Avalonia.Controls;

using ProjectAvalonia.Common.Helpers;

using ReactiveUI;

namespace ProjectAvalonia.Behaviors;

public class RegisterNotificationHostBehavior : DisposingBehavior<Window>
{
    protected override void OnAttached(CompositeDisposable disposables)
    {
        if (AssociatedObject is null)
        {
            return;
        }

        NotificationHelpers.SetNotificationManager(AssociatedObject);

        // Must set notification host again after theme changing.
        Observable
            .FromEventPattern(AssociatedObject, nameof(AssociatedObject.ResourcesChanged))
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(_ => NotificationHelpers.SetNotificationManager(AssociatedObject))
            .DisposeWith(disposables);
    }
}
