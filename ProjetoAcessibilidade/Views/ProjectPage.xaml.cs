using System.Diagnostics;

using Microsoft.UI.Xaml.Controls;

using ProjetoAcessibilidade.TemplateSelector;
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

    private void TreeViewItem_DragOver(object sender, Microsoft.UI.Xaml.DragEventArgs e)
    {
        var result = e.OriginalSource;

        if (sender is TreeViewItem item)
        {
            Debug.WriteLine((item.DataContext as ExplorerItem).Name);
            //Debug.WriteLine(sender);
        }
    }
}
