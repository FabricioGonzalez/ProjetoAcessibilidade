using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ProjectAvalonia.Features.Settings.Views;

public partial class GeneralSettingsTabView : UserControl
{
    public GeneralSettingsTabView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
