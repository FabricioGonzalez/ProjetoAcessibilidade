using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Input;

namespace ProjectAvalonia.Behaviors;

internal class SwallowEnterKeyBehavior : AttachedToVisualTreeBehavior<InputElement>
{
    protected override void OnAttachedToVisualTree(
        CompositeDisposable disposable
    )
    {
        if (AssociatedObject is null)
        {
            return;
        }

        Observable
            .FromEventPattern<KeyEventArgs>(target: AssociatedObject, eventName: nameof(InputElement.KeyDown))
            .Where(predicate: args => args.EventArgs.Key == Key.Enter)
            .Subscribe(onNext: r => r.EventArgs.Handled = true)
            .DisposeWith(compositeDisposable: disposable);
    }
}