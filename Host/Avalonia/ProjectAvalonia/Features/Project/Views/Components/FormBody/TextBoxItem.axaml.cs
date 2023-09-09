using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ProjectAvalonia.Features.Project.Views.Components;

public partial class TextBoxItem : UserControl
{
    public static readonly AttachedProperty<string> TopicProperty =
        AvaloniaProperty.RegisterAttached<TextBoxItem, UserControl, string>(name: nameof(Topic));

    public static readonly AttachedProperty<string> TextDataProperty =
        AvaloniaProperty.RegisterAttached<TextBoxItem, UserControl, string>(name: nameof(TextData));

    public static readonly AttachedProperty<string> MeasurementUnitProperty =
        AvaloniaProperty.RegisterAttached<TextBoxItem, UserControl, string>(name: nameof(MeasurementUnit));

    public TextBoxItem()
    {
        InitializeComponent();
    }

    public string? Topic
    {
        get => GetValue(property: TopicProperty);
        set => SetValue(property: TopicProperty, value: value);
    }

    public string? TextData
    {
        get => GetValue(property: TextDataProperty);
        set => SetValue(property: TextDataProperty, value: value);
    }

    public string? MeasurementUnit
    {
        get => GetValue(property: MeasurementUnitProperty);
        set => SetValue(property: MeasurementUnitProperty, value: value);
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(obj: this);
}