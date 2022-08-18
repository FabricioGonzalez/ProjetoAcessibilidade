using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using SystemApplication.Services.UIOutputs;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CustomControls;
public sealed partial class ProjectItemTemplate : UserControl
{
    public ItemModel ProjectItem
    {
        get => (ItemModel)GetValue(ProjectItemProperty);
        set => SetValue(ProjectItemProperty, value);
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ProjectItemProperty =
        DependencyProperty.Register("ProjectItem", typeof(ItemModel), typeof(ProjectItemTemplate), new PropertyMetadata(0));


    public ProjectItemTemplate()
    {
        InitializeComponent();
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        ProjectItem.IsEditing = true;
    }

    private void CheckBox_Checked(object sender, RoutedEventArgs e)
    {
        ProjectItem.IsEditing = true;
    }

    private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
    {
        ProjectItem.IsEditing = true;
    }
}
