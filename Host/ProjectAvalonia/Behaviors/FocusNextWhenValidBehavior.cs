using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using ReactiveUI;

namespace ProjectAvalonia.Behaviors;

public class FocusNextWhenValidBehavior : DisposingBehavior<TextBox>
{
    protected override void OnAttached(
        CompositeDisposable disposables
    )
    {
        if (AssociatedObject is null)
        {
            return;
        }

        var hasErrors = AssociatedObject.GetObservable(property: DataValidationErrors.HasErrorsProperty);
        var text = AssociatedObject.GetObservable(property: TextBox.TextProperty);

        hasErrors.Select(selector: _ => Unit.Default)
            .Merge(second: text.Select(selector: _ => Unit.Default))
            .Throttle(dueTime: TimeSpan.FromMilliseconds(value: 100))
            .ObserveOn(scheduler: RxApp.MainThreadScheduler)
            .Subscribe(onNext: _ =>
            {
                if (AssociatedObject is not null &&
                    !DataValidationErrors.GetHasErrors(control: AssociatedObject) &&
                    !string.IsNullOrEmpty(value: AssociatedObject.Text) &&
                    KeyboardNavigationHandler.GetNext(element: AssociatedObject, direction: NavigationDirection.Next) is
                        { } nextFocus)
                {
                    nextFocus.Focus();
                }
            })
            .DisposeWith(compositeDisposable: disposables);
    }
}