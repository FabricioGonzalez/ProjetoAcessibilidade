using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;

namespace ProjectAvalonia.Common.Controls;

public partial class FormItemContent : UserControl
{
    public static readonly DirectProperty<FormItemContent, string> LabelProperty =
        AvaloniaProperty.RegisterDirect<FormItemContent, string>(
            name: nameof(Label),
            getter: content => content.Label,
            setter: (
                content
                , s
            ) => content.Label = s,
            defaultBindingMode: BindingMode.OneTime);

    public static readonly DirectProperty<FormItemContent, string> ContentTextProperty =
        AvaloniaProperty.RegisterDirect<FormItemContent, string>(
            name: nameof(ContentText),
            getter: content => content.Label,
            setter: (
                content
                , s
            ) => content.ContentText = s,
            defaultBindingMode: BindingMode.TwoWay);

    public FormItemContent()
    {
        InitializeComponent();
    }


    public string Label
    {
        get => GetValue(LabelProperty);
        set => SetValue(property: LabelProperty, value: value);
    }


    public string ContentText
    {
        get => GetValue(ContentTextProperty);
        set => SetValue(property: ContentTextProperty, value: value);
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}