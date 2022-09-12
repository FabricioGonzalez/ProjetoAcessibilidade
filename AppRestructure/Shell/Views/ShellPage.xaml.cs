using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Runtime.InteropServices.WindowsRuntime;

using AppRestructure.Shell.ViewModels;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

using ReactiveUI;

using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AppRestructure.Shell.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class ShellPage : RoutedViewHost,IViewFor<ShellPageViewModel>
{
    public static readonly DependencyProperty ViewModelProperty = DependencyProperty
.Register(nameof(ViewModel), typeof(ShellPageViewModel), typeof(ShellPage), new PropertyMetadata(null));
    public ShellPageViewModel ViewModel
    {
        get => (ShellPageViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }
    object IViewFor.ViewModel
    {
        get ;
        set ;
    }

    public ShellPage()
    {
        InitializeComponent();
        this.WhenActivated(disposables =>
        {
            // Bind the view model router to RoutedViewHost.Router property.
            this.OneWayBind(ViewModel, x => x.Router, x => x.Router)
                .DisposeWith(disposables);
            //this.BindCommand(ViewModel, x => x.GoNext, x => x.GoNextButton)
            //    .DisposeWith(disposables);
            //this.BindCommand(ViewModel, x => x.GoBack, x => x.GoBackButton)
            //    .DisposeWith(disposables);
        });
    }
}
