using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using ReactiveUI;

namespace ProjectAvalonia.Behaviors;

public class BindableFlyoutOpenBehavior : DisposingBehavior<Control>
{
    public static readonly StyledProperty<bool> IsOpenProperty =
        AvaloniaProperty.Register<BindableFlyoutOpenBehavior, bool>(name: nameof(IsOpen));

    public bool IsOpen
    {
        get => GetValue(property: IsOpenProperty);
        set => SetValue(property: IsOpenProperty, value: value);
    }

    protected override void OnAttached(
        CompositeDisposable disposable
    )
    {
        if (AssociatedObject is null)
        {
            return;
        }

        Observable
            .FromEventPattern(target: AssociatedObject, eventName: nameof(AssociatedObject.PointerEnter))
            .Subscribe(onNext: _ => IsOpen = true)
            .DisposeWith(compositeDisposable: disposable);

        this.WhenAnyValue(property1: x => x.IsOpen)
            .Subscribe(onNext: isOpen =>
            {
                if (isOpen)
                {
                    FlyoutBase.ShowAttachedFlyout(flyoutOwner: AssociatedObject);
                }
                else
                {
                    FlyoutBase.GetAttachedFlyout(element: AssociatedObject)?.Hide();
                }
            })
            .DisposeWith(compositeDisposable: disposable);
    }
}