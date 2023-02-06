using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ProjectAvalonia.Views.Dialogs;

public partial class ShowErrorDialogView : UserControl
{
    public ShowErrorDialogView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
