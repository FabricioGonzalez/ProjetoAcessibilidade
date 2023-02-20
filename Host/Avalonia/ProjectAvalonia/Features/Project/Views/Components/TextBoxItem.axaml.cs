using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ProjectAvalonia.Features.Project.Views.Components;
public partial class TextBoxItem : UserControl
{
    public static readonly AttachedProperty<string> TopicProperty =
        AvaloniaProperty.RegisterAttached<TextBoxItem, UserControl, string>(nameof(Topic));
    public string? Topic
    {
        get => GetValue(TopicProperty);
        set => SetValue(TopicProperty, value);
    }

    public static readonly AttachedProperty<string> TextDataProperty =
        AvaloniaProperty.RegisterAttached<TextBoxItem, UserControl, string>(nameof(TextData));
    public string? TextData
    {
        get => GetValue(TextDataProperty);
        set => SetValue(TextDataProperty, value);
    }

    public static readonly AttachedProperty<string> MeasurementUnitProperty =
        AvaloniaProperty.RegisterAttached<TextBoxItem, UserControl, string>(nameof(MeasurementUnit));
    public string? MeasurementUnit
    {
        get => GetValue(MeasurementUnitProperty);
        set => SetValue(MeasurementUnitProperty, value);
    }

    public TextBoxItem()
    {
        InitializeComponent();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
