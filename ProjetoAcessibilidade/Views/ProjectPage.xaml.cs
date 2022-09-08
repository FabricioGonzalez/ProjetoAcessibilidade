using System.Diagnostics;
using System.IO;
using System.Linq;

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
        InitializeComponent();
        ViewModel = App.GetService<ProjectViewModel>();
        DataContext = ViewModel;
    }

    //private void TextBox_LostFocus(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    //{
    //    var result = ((TextBox)e.OriginalSource).Text;

    //}

    //private void TreeViewItem_Drop(object sender, Microsoft.UI.Xaml.DragEventArgs e)
    //{
    //    var result = e.OriginalSource;

    //    if (sender is TreeViewItem item)
    //    {
    //        Debug.WriteLine((item.DataContext as ExplorerItem).Name);
    //        //Debug.WriteLine(sender);
    //    }
    //}

    //private void ProjectEditingTabView_Drop(object sender, Microsoft.UI.Xaml.DragEventArgs e)
    //{
    //    var result = e.OriginalSource;

    //    if (sender is TreeViewItem item)
    //    {
    //        Debug.WriteLine((item.DataContext as ExplorerItem).Name);
    //        //Debug.WriteLine(sender);
    //    }
    //}

    //private void TreeViewItem_DoubleTapped(object sender, Microsoft.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
    //{
    //    if (sender is TreeViewItem item)
    //    {
    //        Debug.WriteLine((item.DataContext as ExplorerItem).Name);

    //        ViewModel.AddItemCommand.Execute((item.DataContext as ExplorerItem));
    //    }
    //}

    //private void ContentArea_DropCompleted(Microsoft.UI.Xaml.UIElement sender, Microsoft.UI.Xaml.DropCompletedEventArgs args)
    //{
    //    Debug.WriteLine(args.DropResult);
    //}

    //private void AddItemItem_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    //{
    //    try
    //    {

    //        if (sender is MenuFlyoutItem itemFlyout)
    //        {
    //            var item = (ExplorerItem)itemFlyout.DataContext;
    //            if (item.Type == ExplorerItem.ExplorerItemType.Folder)
    //                ViewModel.AddItemToProjectCommand.Execute(item);
    //        }
    //    }
    //    catch (System.Exception)
    //    {

    //        throw;
    //    }
    //}
    //private void AddFolderItem_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    //{
    //    if (sender is MenuFlyoutItem itemFlyout)
    //    {
    //        var item = (ExplorerItem)itemFlyout.DataContext;

    //        var res = new ExplorerItem()
    //        {
    //            Name = "",
    //            Path = Path.Combine(item.Path, string.Empty)
    //        };

    //        item.Children.Add(res);
    //        res.IsEditing = true;

    //        //ViewModel.AddFolderToProjectCommand.Execute(res);
    //    }
    //}
    //private void RenameItem_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    //{
    //    if (sender is MenuFlyoutItem itemFlyout)
    //    {
    //        var item = (ExplorerItem)itemFlyout.DataContext;

    //        ViewModel.Items.Any(i =>
    //        {
    //            if (i.Type is ExplorerItem.ExplorerItemType.Folder)
    //            {
    //                i.Children.Any(e =>
    //                {
    //                    if (e.IsEditing)
    //                    {
    //                        e.IsEditing = false;
    //                        return true;
    //                    }
    //                    return false;
    //                });
    //            }

    //            if (i.IsEditing)
    //            {
    //                i.IsEditing = false;
    //                return true;
    //            }
    //            return false;
    //        });

    //        item.IsEditing = true;
    //    }
    //}
    //private void ExcludeItem_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    //{
    //    if (sender is MenuFlyoutItem itemFlyout)
    //    {
    //        var item = (ExplorerItem)itemFlyout.DataContext;

    //        item.IsEditing = true;

    //        ViewModel.Items.Remove(item);
    //    }
    //}
    //private void TextBox_KeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
    //{
    //    if (sender is TextBox textbox)
    //    {
    //        if (e.Key == Windows.System.VirtualKey.Enter)
    //        {
    //            var item = (ExplorerItem)textbox.DataContext;
    //            if (textbox.Text.Length > 0)
    //            {
    //                item.Name = textbox.Text;

    //                item.IsEditing = false;
    //                item.NewName = string.Empty;

    //                //ViewModel.
    //            }
    //        }
    //    }
    //}
}
