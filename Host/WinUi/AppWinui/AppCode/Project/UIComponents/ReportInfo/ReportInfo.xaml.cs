using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using AppWinui.AppCode.Project.UIComponents.TreeView;
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

namespace AppWinui.AppCode.Project.UIComponents.ReportInfo;
public sealed partial class ReportInfo : UserControl
{
    public ProjectViewModel ViewModel
    {
        get => (ProjectViewModel)GetValue(ViewModelPropery);
        set => SetValue(ViewModelPropery, value);
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ViewModelPropery =
        DependencyProperty.Register(nameof(ViewModel), typeof(ProjectViewModel), typeof(ReportInfo), new PropertyMetadata(null));

    public ReportInfo()
    {
        InitializeComponent();
    }
}
