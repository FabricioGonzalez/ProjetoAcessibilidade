using AppUsecases.Project.Entities.FileTemplate;
using AppUsecases.Project.Enums;

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
          
    }
    private void renameFile_Click(object sender, RoutedEventArgs e)
    {
        if (sender is MenuFlyoutItem itemFlyout)
        {
            var item = (ExplorerItem)itemFlyout.DataContext;

            ExplorerViewModel.ExplorerState.Items.Any(i =>
            {
                if (i is FolderItem folder)
                {
                    folder.Children.Any(e =>
                    {
                      
                        return false;
                    });
                }

                return false;
            });
        }
    }

    private void excludeFile_Click(object sender, RoutedEventArgs e)
    {
        if (sender is MenuFlyoutItem itemFlyout)
        {
            var item = (ExplorerItem)itemFlyout.DataContext;

            ExplorerViewModel.ExplorerState.Items.Remove(item);
        }
    }

    private void renameFolder_Click(object sender, RoutedEventArgs e)
    {
        if (sender is MenuFlyoutItem itemFlyout)
        {
            var item = (ExplorerItem)itemFlyout.DataContext;

            ExplorerViewModel.ExplorerState.Items.Any(i =>
            {
                if (i is FolderItem folder)
                {
                    folder.Children.Any(e =>
                    {
                        return false;
                    });
                }

                return false;
            });
        }
    }

    private void excludeFolder_Click(object sender, RoutedEventArgs e)
    {
        if (sender is MenuFlyoutItem itemFlyout)
        {
            var item = (ExplorerItem)itemFlyout.DataContext;

            ExplorerViewModel.ExplorerState.Items.Remove(item);
        }
    }

    private void addFile_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (sender is MenuFlyoutItem itemFlyout)
            {
                var item = (ExplorerItem)itemFlyout.DataContext;
                if (item is FolderItem folder)
                    ExplorerViewModel.AddItemCommand.Execute(folder);
            }
            
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    private void addFolder_Click(object sender, RoutedEventArgs e)
    {
        if(sender is MenuFlyoutItem itemFlyout)
        {
            var item = (FolderItem)itemFlyout.DataContext;

            var res = new ExplorerItem()
            {
                Name = "",
                Path = Path.Combine(item.Path, string.Empty)
            };

            item.Children.Add(res);

            //ViewModel.AddFolderToProjectCommand.Execute(res);
        }
    }
}
