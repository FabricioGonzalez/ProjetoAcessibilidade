using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ProjectAvalonia.Features.Project.Dialogs;

public partial class DeleteDialogView : UserControl
{
    public DeleteDialogView()
    {
        InitializeComponent();
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(obj: this);
}