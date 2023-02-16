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
    protected override void OnAttachedToVisualTree(CompositeDisposable disposable)
    {
        if (AssociatedObject is null)
        {
            return;
        }

        var gotFocus = AssociatedObject.OnEvent(InputElement.GotFocusEvent);
        var lostFocus = AssociatedObject.OnEvent(InputElement.LostFocusEvent);
        var isFocused = gotFocus.Select(_ => true).Merge(lostFocus.Select(_ => false));

        isFocused
            .Throttle(TimeSpan.FromSeconds(0.1))
            .DistinctUntilChanged()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Where(focused => focused)
            .Do(_ => AssociatedObject.SelectAll())
            .Subscribe()
            .DisposeWith(disposable);
    }
}
