using AppUsecases.Entities.FileTemplate;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AppWinui.AppCode.Project.UIComponents.TreeView;
public sealed partial class FileTreeViewItemTemplate : UserControl
{
    public ExplorerItem Item
    {
        get => (ExplorerItem)GetValue(ItemProperty);
        set => SetValue(ItemProperty, value);
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ItemProperty =
        DependencyProperty.Register("Item", typeof(ExplorerItem), typeof(FileTreeViewItemTemplate), new PropertyMetadata(null));


    public FileTreeViewItemTemplate()
    {
        InitializeComponent();
    }
}
