using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.VisualTree;
using ProjectAvalonia.Common.Extensions;
using ReactiveUI;

namespace ProjectAvalonia.Behaviors;

public class HoldKeyBehavior : AttachedToVisualTreeBehavior<InputElement>
{
    public static readonly StyledProperty<Key?> KeyProperty =
        AvaloniaProperty.Register<HoldKeyBehavior, Key?>(name: nameof(Key));

    public static readonly StyledProperty<bool> IsKeyPressedProperty =
        AvaloniaProperty.Register<HoldKeyBehavior, bool>(name: nameof(IsKeyPressed)
            , defaultBindingMode: BindingMode.TwoWay);

    public Key? Key
    {
        get => GetValue(property: KeyProperty);
        set => SetValue(property: KeyProperty, value: value);
    }

    public bool IsKeyPressed
    {
        get => GetValue(property: IsKeyPressedProperty);
        set => SetValue(property: IsKeyPressedProperty, value: value);
    }

    protected override void OnAttachedToVisualTree(
        CompositeDisposable disposable
    )
    {
        if (AssociatedObject.GetVisualRoot() is not IInputElement ie)
        {
            return;
        }

        var ups = ie.OnEvent(routedEvent: InputElement.KeyDownEvent);
        var downs = ie.OnEvent(routedEvent: InputElement.KeyUpEvent);

        var keyEvents = ups
            .Select(selector: x => new { x.EventArgs.Key, IsPressed = true })
            .Merge(second: downs.Select(selector: x => new { x.EventArgs.Key, IsPressed = false }));

        var targetKeys = this.WhenAnyValue(property1: x => x.Key);

        keyEvents
            .WithLatestFrom(second: targetKeys)
            .Select(selector: x => (PressedKey: x.First.Key, x.First.IsPressed, TargetKey: x.Second))
            .Where(predicate: x => x.PressedKey == x.TargetKey)
            .Select(selector: x => x.IsPressed)
            .StartWith(false)
            .Do(onNext: b => IsKeyPressed = b)
            .Subscribe()
            .DisposeWith(compositeDisposable: disposable);
    }
}