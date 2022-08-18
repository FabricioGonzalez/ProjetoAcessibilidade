using System.Diagnostics;

using CustomControls.TemplateSelectors;

using Microsoft.UI.Xaml.Controls;

using ProjetoAcessibilidade.ViewModels;

using SystemApplication.Services.UIOutputs;

namespace ProjetoAcessibilidade.Views;

public sealed partial class ProjectPage : Page
{
    public ProjectViewModel ViewModel
    {
        get;
    }

    public ProjectPage()
    {
        ViewModel = App.GetService<ProjectViewModel>();
        InitializeComponent();
    }

    private void TextBox_LostFocus(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var result = ((TextBox)e.OriginalSource).Text;

        ViewModel.TextBoxLostFocusCommand.Execute(result);
    }

    private void TreeViewItem_Drop(object sender, Microsoft.UI.Xaml.DragEventArgs e)
    {
        var result = e.OriginalSource;

        if (sender is TreeViewItem item)
        {
            Debug.WriteLine((item.DataContext as ExplorerItem).Name);
            //Debug.WriteLine(sender);
        }
    }

    private void ProjectEditingTabView_Drop(object sender, Microsoft.UI.Xaml.DragEventArgs e)
    {
        var result = e.OriginalSource;

        if (sender is TreeViewItem item)
        {
            Debug.WriteLine((item.DataContext as ExplorerItem).Name);
            //Debug.WriteLine(sender);
        }
    }

    private void TreeViewItem_DoubleTapped(object sender, Microsoft.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
    {
        if (sender is TreeViewItem item)
        {
            Debug.WriteLine((item.DataContext as ExplorerItem).Name);

            ViewModel.AddItemCommand.Execute((item.DataContext as ExplorerItem));
        }
    }

    private void ContentArea_DropCompleted(Microsoft.UI.Xaml.UIElement sender, Microsoft.UI.Xaml.DropCompletedEventArgs args)
    {
        Debug.WriteLine(args.DropResult);
    }

    private void AddItemItem_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        if (sender is MenuFlyoutItem itemFlyout)
        {
            var item = (ExplorerItem)itemFlyout.DataContext;
            if (item.Type == ExplorerItem.ExplorerItemType.Folder)
                ViewModel.AddItemToProjectCommand.Execute(item);
        }
    }
    private void AddFolderItem_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        if (sender is MenuFlyoutItem itemFlyout)
        {
            var item = (ExplorerItem)itemFlyout.DataContext;

            var res = new ExplorerItem()
            {
                Name = "",
                Path = "D:\\Programacao\\Projetos\\Desktop\\C#\\ProjetoAcessibilidade\\Data\\Tables\\Entradas Portas Externo.xml"
            };

            item.Children.Add(res);
            item.IsEditing = true;
        }
    }
    private void RenameItem_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        if (sender is MenuFlyoutItem itemFlyout)
        {
            var item = (ExplorerItem)itemFlyout.DataContext;
            item.IsEditing = true;
        }
    }
    private void ExcludeItem_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        if (sender is MenuFlyoutItem itemFlyout)
        {
            var item = (ExplorerItem)itemFlyout.DataContext;

            item.IsEditing = true;

            ViewModel.Items.Remove(item);
        }
    }
    private void TextBox_KeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
    {
        if (sender is TextBox textbox)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                var item = (ExplorerItem)textbox.DataContext;

                item.Name = textbox.Text;

                item.IsEditing = false;
                item.NewName = string.Empty;

                //ViewModel.
            }
        }
    }
}
