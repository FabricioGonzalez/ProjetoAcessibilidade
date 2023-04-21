using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ProjectAvalonia.Features.Project.Views.Components;

public partial class OptionsItem : UserControl
{
    /*public static readonly AttachedProperty<ObservableCollection<OptionsItemState>> OptionsProperty =
        AvaloniaProperty.RegisterAttached<OptionsItem, UserControl, ObservableCollection<OptionsItemState>>(
            name: nameof(Options));

    public OptionsItem()
    {
        InitializeComponent();
    }

    public ObservableCollection<OptionsItemState>? Options
    {
        get => GetValue(property: OptionsProperty);
        set => SetValue(property: OptionsProperty, value: value);
    }*/

    private void InitializeComponent() => AvaloniaXamlLoader.Load(obj: this);
}