
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using SystemApplication.Services.UIOutputs;

namespace CustomControls;

public sealed partial class ProjectEditingTabViewItem : TabViewItem
{
    public string itemPath
    {
        get => (string)GetValue(itemPathProperty);
        set => SetValue(itemPathProperty, value);
    }

    // Using a DependencyProperty as the backing store for itemPath.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty itemPathProperty =
        DependencyProperty.Register("itemPath", typeof(string), typeof(ProjectEditingTabViewItem), new PropertyMetadata(""));
    public ItemModel ProjectItem
    {
        get => (ItemModel)GetValue(ProjectItemProperty);
        set => SetValue(ProjectItemProperty, value);
    }
    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ProjectItemProperty =
        DependencyProperty.Register("ProjectItem", typeof(ItemModel), typeof(ProjectEditingTabViewItem), new PropertyMetadata(0));


    public ProjectEditingTabViewItem()
    {
        InitializeComponent();
    }
}
