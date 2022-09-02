using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

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
            typeof(ProjectEditingTabView), new PropertyMetadata(new ObservableCollection<ProjectEditingTabViewItem>(), OnItemsChanged));

    private static void OnItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ProjectEditingTabView item)
        {
            //item.
        }
    }


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
        DragEnter += new DragEventHandler(XmlEditControl_DragEnter);
        Drop += new DragEventHandler(this.XmlEditControl_DragDrop);
    }
}
