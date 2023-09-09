using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using ReactiveUI;

namespace ProjectAvalonia.Behaviors;

internal class FocusNextItemBehavior : DisposingBehavior<Control>
{
    public static readonly StyledProperty<bool> IsFocusedProperty =
        AvaloniaProperty.Register<FocusNextItemBehavior, bool>(name: nameof(IsFocused), defaultValue: true);

    public bool IsFocused
    {
        get => GetValue(property: IsFocusedProperty);
        set => SetValue(property: IsFocusedProperty, value: value);
    }

    protected override void OnAttached(
        CompositeDisposable disposables
    ) =>
        this.WhenAnyValue(property1: x => x.IsFocused)
            .Where(predicate: x => x == false)
            .Subscribe(
                onNext: _ =>
                {
                    var parentControl = AssociatedObject.FindLogicalAncestorOfType<ItemsControl>();

                    if (parentControl is not null)
                    {
                        foreach (var item in parentControl.GetLogicalChildren())
                        {
                            var nextToFocus = item.FindLogicalDescendantOfType<TextBox>();

                            if (nextToFocus.IsEnabled)
                            {
                                nextToFocus.Focus();
                                return;
                            }
                        }

                        parentControl.Focus();
                    }
                })
            .DisposeWith(compositeDisposable: disposables);
}