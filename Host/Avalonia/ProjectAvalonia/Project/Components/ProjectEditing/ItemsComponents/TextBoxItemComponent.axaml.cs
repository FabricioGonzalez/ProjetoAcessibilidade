using Avalonia;
using Avalonia.Controls;

using ReactiveUI;

namespace ProjectAvalonia.Project.Components.ProjectEditing.ItemsComponents;
public partial class TextBoxItemComponent : UserControl, IActivatableView
{
    public static readonly AttachedProperty<string> TopicProperty =
        AvaloniaProperty.RegisterAttached<TextBoxItemComponent, UserControl, string>(nameof(Topic));
    public string? Topic
    {
        get => GetValue(TopicProperty);
        set => SetValue(TopicProperty, value);
    }

    public static readonly AttachedProperty<string> TextDataProperty =
        AvaloniaProperty.RegisterAttached<TextBoxItemComponent, UserControl, string>(nameof(TextData));
    public string? TextData
    {
        get => GetValue(TextDataProperty);
        set => SetValue(TextDataProperty, value);
    }

    public static readonly AttachedProperty<string> MeasurementUnitProperty =
        AvaloniaProperty.RegisterAttached<TextBoxItemComponent, UserControl, string>(nameof(MeasurementUnit));
    public string? MeasurementUnit
    {
        get => GetValue(MeasurementUnitProperty);
        set => SetValue(MeasurementUnitProperty, value);
    }


    public TextBoxItemComponent()
    {
        InitializeComponent();
    }
}
