using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Runtime.InteropServices.WindowsRuntime;

using AppRestructure.Home.ViewModels;

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

namespace AppRestructure.Home.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class HomePage : Page, IViewFor<HomeViewModel>
{

    public static readonly DependencyProperty ViewModelProperty = DependencyProperty
     .Register(nameof(ViewModel), typeof(HomeViewModel), typeof(HomePage), new PropertyMetadata(null));

    public HomePage()
    {
        InitializeComponent();
        ViewModel = new HomeViewModel();

        // Setup the bindings.
        // Note: We have to use WhenActivated here, since we need to dispose the
        // bindings on XAML-based platforms, or else the bindings leak memory.
        this.WhenActivated(disposable =>
        {
            this.Bind(ViewModel, x => x.TheText, x => x.TheTextBox.Text)
                .DisposeWith(disposable);
            this.OneWayBind(ViewModel, x => x.TheText, x => x.TheTextBlock.Text)
                .DisposeWith(disposable);
            this.BindCommand(ViewModel, x => x.TheTextCommand, x => x.TheTextButton)
                .DisposeWith(disposable);
        });
    }
    public HomeViewModel ViewModel
    {
        get => (HomeViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    object IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (HomeViewModel)value;
    }
}
