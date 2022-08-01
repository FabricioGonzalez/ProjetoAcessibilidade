using System.Collections.ObjectModel;
using System.Diagnostics;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using Windows.Devices.Enumeration;

namespace CustomControls;

public sealed partial class ProjectEditingTabView : UserControl
{
    public ObservableCollection<TabViewItem> Items
    {
        get => (ObservableCollection<TabViewItem>)GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ItemsProperty =
        DependencyProperty.Register("Items", typeof(ObservableCollection<TabViewItem>),
            typeof(ProjectEditingTabView), new PropertyMetadata(new ObservableCollection<TabViewItem>()));

    private void TabView_AddTabButtonClick(TabView sender, object args)
    {
        var newTab = new TabViewItem();
        newTab.IconSource = new SymbolIconSource() { Symbol = Symbol.Document };
        newTab.Header = "New Document";

        Items.Add(newTab);
    }

    // Remove the requested tab from the TabView
    private void TabView_TabCloseRequested(TabView sender, TabViewTabCloseRequestedEventArgs args)
    {
        Items.Remove(args.Tab);
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
