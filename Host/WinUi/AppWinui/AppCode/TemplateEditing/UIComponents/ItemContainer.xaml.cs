using AppUsecases.Entities;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AppWinui.AppCode.TemplateEditing.UIComponents;
public sealed partial class ItemContainer : UserControl
{
    public AppItemModel ProjectItem
    {
        get => (AppItemModel)GetValue(ProjectItemProperty);
        set => SetValue(ProjectItemProperty, value);
    }
    public static readonly DependencyProperty ProjectItemProperty =
       DependencyProperty.Register(nameof(ProjectItem), typeof(AppItemModel), typeof(ItemContainer), new PropertyMetadata(new AppItemModel()));

    public ItemContainer()
    {
        InitializeComponent();
    }

    private void AppBarToggleButton_Checked(object sender, RoutedEventArgs e)
    {

    }
}
