using AppWinui.AppCode.Project.ViewModels;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using ReactiveUI;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AppWinui.AppCode.Project.UIComponents.TreeView;
public sealed partial class ExplorerItemTreeView : UserControl
{
    public ExplorerViewViewModel ExplorerViewModel
    {
        get => (ExplorerViewViewModel)GetValue(ExplorerViewViewModelPropery);
        set => SetValue(ExplorerViewViewModelPropery, value);
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ExplorerViewViewModelPropery =
        DependencyProperty.Register("ExplorerViewModel",
            typeof(ExplorerViewViewModel),
            typeof(ExplorerItemTreeView),
            new PropertyMetadata(null, (s, e) =>
            {
                if (s is ExplorerItemTreeView explorer)
                {
                    explorer.DataContext = e.NewValue;
                }
            }));


    public ExplorerItemTreeView()
    {
        InitializeComponent();

        DataContext = ExplorerViewModel;

        this.WhenAnyValue(x => x.ExplorerViewModel.ExplorerState.Items)
            .WhereNotNull()
            .BindTo(this, x => x.projectExplorer.ItemsSource);
    }
}
