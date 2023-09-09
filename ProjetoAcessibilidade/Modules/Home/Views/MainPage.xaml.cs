using System.Reactive.Disposables;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ProjetoAcessibilidade.Modules.Home.ViewModels;

using ReactiveUI;

namespace ProjetoAcessibilidade.Views;

public sealed partial class MainPage : Page, IViewFor<MainViewModel>
{

    public static readonly DependencyProperty ViewModelProperty = DependencyProperty
    .Register(nameof(ViewModel), typeof(MainViewModel), typeof(MainPage), new PropertyMetadata(null));
    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();

        this.WhenActivated(disposable =>
        {
            this.Bind(ViewModel, x => x.Items, x => x.listRecentOpened.ItemsSource)
                .DisposeWith(disposable);
            //this.OneWayBind(ViewModel, x => x.TheText, x => x.TheTextBlock.Text)
            //    .DisposeWith(disposable);
            //this.BindCommand(ViewModel, x => x.TheTextCommand, x => x.TheTextButton)
            //    .DisposeWith(disposable);
        });

    }
    public MainViewModel ViewModel
    {
        get => (MainViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    object IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (MainViewModel)value;
    }
}
