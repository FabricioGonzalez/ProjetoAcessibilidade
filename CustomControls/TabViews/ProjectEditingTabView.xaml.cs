using System.Collections.ObjectModel;
using System.Diagnostics;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using Windows.Devices.Enumeration;

namespace CustomControls;

public sealed partial class ProjectEditingTabView : UserControl
{
    public ObservableCollection<ProjectEditingTabViewItem> Items
    {
        get => (ObservableCollection<ProjectEditingTabViewItem>)GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ItemsProperty =
        DependencyProperty.Register("Items", typeof(ObservableCollection<TabViewItem>),
            typeof(ProjectEditingTabView), new PropertyMetadata(new ObservableCollection<ProjectEditingTabViewItem>()));


    // Remove the requested tab from the TabView
    private void TabView_TabCloseRequested(TabView sender, TabViewTabCloseRequestedEventArgs args)
    {
        Items.Remove(args.Tab as ProjectEditingTabViewItem);
    }

    private void XmlEditControl_DragEnter(object sender, DragEventArgs e)
    {
        Debug.WriteLine("I entered");
    }

    private void XmlEditControl_DragDrop(object sender, DragEventArgs e)
    {
        Debug.WriteLine("I dropped");
    }

    public ProjectEditingTabView()
    {
        InitializeComponent();
        this.DragEnter += new DragEventHandler(XmlEditControl_DragEnter);
        this.Drop += new DragEventHandler(this.XmlEditControl_DragDrop);
    }
}
