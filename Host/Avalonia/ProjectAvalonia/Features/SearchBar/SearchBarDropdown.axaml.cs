using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ProjectAvalonia.Features.SearchBar;

public partial class SearchBarDropdown : UserControl
{
    public SearchBarDropdown()
    {
        InitializeComponent();
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(obj: this);
}