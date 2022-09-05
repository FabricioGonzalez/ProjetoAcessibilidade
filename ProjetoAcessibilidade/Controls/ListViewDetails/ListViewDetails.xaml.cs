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

using SystemApplication.Services.UIOutputs;

using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ProjetoAcessibilidade.Controls.ListViewDetails;
public sealed partial class ListViewDetails : UserControl
{
    public ItemModel ProjectItem
    {
        get => (ItemModel)GetValue(ProjectItemProperty);
        set => SetValue(ProjectItemProperty, value);
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ProjectItemProperty =
        DependencyProperty.Register("ProjectItem", typeof(ItemModel), typeof(ListViewDetails), new PropertyMetadata(new ItemModel()));

    private static void OnListDetailsMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = d as ListViewDetails;
        control.ForegroundElement.ChangeView(0, 0, 1);
    }
    public ListViewDetails()
    {
        InitializeComponent();
    }

    private void AppBarButton_Click(object sender, RoutedEventArgs e)
    {

    }
}
