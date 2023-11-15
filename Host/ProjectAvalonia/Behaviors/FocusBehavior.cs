using System;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;

namespace ProjectAvalonia.Behaviors;

internal class FocusBehavior : DisposingBehavior<Control>
{
    public static readonly StyledProperty<bool> IsFocusedProperty =
        AvaloniaProperty.Register<FocusBehavior, bool>(name: nameof(IsFocused), defaultBindingMode: BindingMode.TwoWay);

    public bool IsFocused
    {
        get => GetValue(property: IsFocusedProperty);
        set => SetValue(property: IsFocusedProperty, value: value);
    }

    protected override void OnAttached(
        CompositeDisposable disposables
    )
    {
        base.OnAttached();

        if (AssociatedObject is not null)
        {
            AssociatedObject.AttachedToLogicalTree += (
                    sender
                    , e
                ) =>
                disposables.Add(item: this.GetObservable(property: IsFocusedProperty)
                    .Subscribe(onNext: focused =>
                    {
                        if (focused)
                        {
                            AssociatedObject.Focus();
                        }
                    }));
        }
    }
}