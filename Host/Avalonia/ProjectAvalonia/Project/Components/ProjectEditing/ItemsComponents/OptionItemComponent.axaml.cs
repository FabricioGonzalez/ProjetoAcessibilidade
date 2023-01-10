using System;
using System.Diagnostics;
using System.Reactive.Disposables;

using Avalonia;

using Avalonia.Controls;

using ReactiveUI;

namespace ProjectAvalonia.Project.Components.ProjectEditing.ItemsComponents;
public partial class OptionItemComponent : UserControl, IActivatableView
{
    public static readonly AttachedProperty<bool> IsCheckedProperty =
        AvaloniaProperty.RegisterAttached<OptionsComponent, UserControl, bool>(nameof(IsChecked));
    public bool? IsChecked
    {
        get => GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }
    public static readonly AttachedProperty<string> ValueProperty =
        AvaloniaProperty.RegisterAttached<OptionsComponent, UserControl, string>(nameof(Value));
    public string? Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public OptionItemComponent()
    {
        this.WhenActivated((CompositeDisposable disposables) =>
        {
            this.WhenAnyValue(v => v.IsChecked)
            .Subscribe(prop =>
            {
                Debug.WriteLine(prop.GetValueOrDefault());
            });
        });

        InitializeComponent();
    }
}
