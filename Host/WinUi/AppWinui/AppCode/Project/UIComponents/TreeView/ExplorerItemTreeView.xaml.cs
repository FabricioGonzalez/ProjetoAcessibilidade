using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using AppWinui.AppCode.Project.ViewModels;

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

namespace AppWinui.AppCode.Project.UIComponents.TreeView;
public sealed partial class ExplorerItemTreeView : UserControl
{
    public ExplorerViewViewModel ExplorerViewModel
    {
        get
        {
            return (ExplorerViewViewModel)GetValue(ExplorerViewViewModelPropery);
        }
        set
        {
            SetValue(ExplorerViewViewModelPropery, value);
        }
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ExplorerViewViewModelPropery =
        DependencyProperty.Register("ExplorerViewModel", typeof(int), typeof(ExplorerItemTreeView), new PropertyMetadata(null));


    public ExplorerItemTreeView()
    {
        InitializeComponent();
    }
}
