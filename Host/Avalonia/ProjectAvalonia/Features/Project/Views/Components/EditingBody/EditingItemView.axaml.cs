using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace ProjectAvalonia.Features.Project.Views.Components.EditingBody;

public partial class EditingItemView : UserControl
{
    public EditingItemView()
    {
        InitializeComponent();

        HotKeyManager.SetHotKey(this, new KeyGesture(Key.S, KeyModifiers.Control));
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}