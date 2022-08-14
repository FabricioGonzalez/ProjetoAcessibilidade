using System.Diagnostics;

using CustomControls.TemplateSelectors;

using Microsoft.UI.Xaml.Controls;

using ProjetoAcessibilidade.ViewModels;

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

    //private void TreeViewItem_DragOver(object sender, Microsoft.UI.Xaml.DragEventArgs e)
    //{
    //    var result = e.OriginalSource;

    //    if (sender is TreeViewItem item)
    //    {
    //        Debug.WriteLine((item.DataContext as ExplorerItem).Name);
    //        //Debug.WriteLine(sender);
    //    }
    //}

    private void ProjectEditingTabView_Drop(object sender, Microsoft.UI.Xaml.DragEventArgs e)
    {
        var result = e.OriginalSource;

        if (sender is TreeViewItem item)
        {
            Debug.WriteLine((item.DataContext as ExplorerItem).Name);
            //Debug.WriteLine(sender);
        }
    }

    private void ContentArea_Drop(object sender, Microsoft.UI.Xaml.DragEventArgs e)
    {
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

            ViewModel.AddItemCommand.Execute((item.DataContext as ExplorerItem).Name);

        }
    }

    private void MenuFlyoutItem_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {

    }

    private void ContentArea_DropCompleted(Microsoft.UI.Xaml.UIElement sender, Microsoft.UI.Xaml.DropCompletedEventArgs args)
    {
        Debug.WriteLine(args.DropResult);
    }
}
