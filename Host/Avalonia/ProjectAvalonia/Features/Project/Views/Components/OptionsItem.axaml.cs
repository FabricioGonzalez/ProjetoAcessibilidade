using System.Collections.ObjectModel;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

using ProjectAvalonia.Features.Project.States.FormItemState;

namespace ProjectAvalonia.Features.Project.Views.Components;
public partial class OptionsItem : UserControl
{
    public static readonly AttachedProperty<ObservableCollection<OptionsItemState>> OptionsProperty =
        AvaloniaProperty.RegisterAttached<OptionsItem, UserControl, ObservableCollection<OptionsItemState>>(nameof(Options));
    public ObservableCollection<OptionsItemState>? Options
    {
        get => GetValue(OptionsProperty);
        set => SetValue(OptionsProperty, value);
    }
    public OptionsItem()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
