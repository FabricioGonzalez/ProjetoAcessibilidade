
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace CustomControls;

public sealed partial class ProjectEditingTabViewItem : TabViewItem
{


    public string itemPath
    {
        get
        {
            return (string)GetValue(itemPathProperty);
        }
        set
        {
            SetValue(itemPathProperty, value);
        }
    }

    // Using a DependencyProperty as the backing store for itemPath.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty itemPathProperty =
        DependencyProperty.Register("itemPath", typeof(string), typeof(ProjectEditingTabViewItem), new PropertyMetadata(""));


    public ProjectEditingTabViewItem()
    {
        InitializeComponent();
    }
}
