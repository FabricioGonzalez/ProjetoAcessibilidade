using AppUsecases.Project.Entities.FileTemplate;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AppWinui.AppCode.Project.UIComponents.TreeView;
public sealed partial class FolderTreeViewItemTemplate : UserControl
{
    public FolderItem Item
    {
        get => (FolderItem)GetValue(ItemProperty);
        set => SetValue(ItemProperty, value);
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ItemProperty =
        DependencyProperty.Register("Item",
            typeof(FolderItem), 
            typeof(FolderTreeViewItemTemplate),
            new PropertyMetadata(null, (d, args) =>
            {
                if (d is FolderTreeViewItemTemplate itemTemplate)
                {
                    if (args.NewValue != null)
                        itemTemplate.DataContext = args.NewValue;
                }
            }));


    public FolderTreeViewItemTemplate()
    {
        InitializeComponent();
    }
}
