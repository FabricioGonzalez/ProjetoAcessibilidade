using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CustomControls.TabViews;
public sealed partial class ProjectEditingTabViewItemHeaderTemplate : UserControl
{
    public string TextHeader
    {
        get => (string)GetValue(TextHeaderProperty);
        set => SetValue(TextHeaderProperty, value);
    }
    // Using a DependencyProperty as the backing store for TextHeader.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TextHeaderProperty =
        DependencyProperty.Register("TextHeader", typeof(string), typeof(ProjectEditingTabViewItemHeaderTemplate), new PropertyMetadata(""));

    public bool IsEdited
    {
        get => (bool)GetValue(IsEditedProperty);
        set => SetValue(IsEditedProperty, value);
    }
    // Using a DependencyProperty as the backing store for IsEdited.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IsEditedProperty =
        DependencyProperty.Register("IsEdited", typeof(bool), typeof(ProjectEditingTabViewItemHeaderTemplate), new PropertyMetadata(true, IsEditedChanged));

    static void IsEditedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = d as ProjectEditingTabViewItemHeaderTemplate;

        if (control is not null)
        {
            if ((bool)e.NewValue == true)
                control.Icon.Visibility = Visibility.Visible;
            else control.Icon.Visibility = Visibility.Collapsed;
        }
    }

    public ProjectEditingTabViewItemHeaderTemplate()
    {
        this.InitializeComponent();
    }
}
