using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;

namespace ProjectAvalonia.Behaviors;

public class BindPointerOverBehavior : DisposingBehavior<Control>
{
    public static readonly StyledProperty<bool> IsPointerOverProperty =
        AvaloniaProperty.Register<BindPointerOverBehavior, bool>(name: nameof(IsPointerOver)
            , defaultBindingMode: BindingMode.TwoWay);

    public bool IsPointerOver
    {
        get => GetValue(property: IsPointerOverProperty);
        set => SetValue(property: IsPointerOverProperty, value: value);
    }

    protected override void OnAttached(
        CompositeDisposable disposables
    )
    {
        if (AssociatedObject is null)
        {
            return;
        }

        Observable
            .FromEventPattern<AvaloniaPropertyChangedEventArgs>(target: AssociatedObject
                , eventName: nameof(PropertyChanged))
            .Select(selector: x => x.EventArgs)
            .Subscribe(onNext: e =>
            {
                if (e.Property == InputElement.IsPointerOverProperty)
                {
                    IsPointerOver = e.NewValue is true;
                }
            })
            .DisposeWith(compositeDisposable: disposables);

        disposables.Add(item: Disposable.Create(dispose: () => IsPointerOver = false));
    }
}