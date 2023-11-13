using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace ProjectAvalonia.Features.Project.Components.EditingBody;

public partial class EditingItem : UserControl
{
    public EditingItem()
    {
        InitializeComponent();

        HotKeyManager.SetHotKey(target: this, value: new KeyGesture(key: Key.S, modifiers: KeyModifiers.Control));
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}