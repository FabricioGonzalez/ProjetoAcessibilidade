using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using ProjectAvalonia.Common.Extensions;
using ReactiveUI;

namespace ProjectAvalonia.Behaviors;

internal class TextBoxAutoSelectTextBehavior : AttachedToVisualTreeBehavior<TextBox>
{
    protected override void OnAttachedToVisualTree(
        CompositeDisposable disposable
    )
    {
        if (AssociatedObject is null)
        {
            return;
        }

        var gotFocus = AssociatedObject.OnEvent(routedEvent: InputElement.GotFocusEvent);
        var lostFocus = AssociatedObject.OnEvent(routedEvent: InputElement.LostFocusEvent);
        var isFocused = gotFocus.Select(selector: _ => true).Merge(second: lostFocus.Select(selector: _ => false));

        isFocused
            .Throttle(dueTime: TimeSpan.FromSeconds(value: 0.1))
            .DistinctUntilChanged()
            .ObserveOn(scheduler: RxApp.MainThreadScheduler)
            .Where(predicate: focused => focused)
            .Do(onNext: _ => AssociatedObject.SelectAll())
            .Subscribe()
            .DisposeWith(compositeDisposable: disposable);
    }
}