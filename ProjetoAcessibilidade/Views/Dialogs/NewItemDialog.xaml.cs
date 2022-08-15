using System;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using ProjetoAcessibilidade.Helpers;
using ProjetoAcessibilidade.ViewModels.DialogViewModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ProjetoAcessibilidade.Views.Dialogs;
/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class NewItemDialog : Page
{
    public NewItemDialog()
    {
        InitializeComponent();

    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        try
        {
            var window = (DataContext as NewItemViewModel).newItemDialogService.GetDialog();

            if (window is not null)
            {
                if (window.IsTitleBarCustomizable())
                {
                    window.ExtendsContentIntoTitleBar = true;
                    window.SetTitleBar(AppTitleBar);
                    AppTitleBarText.Text = "AppDisplayName".GetLocalized();

                    Grid.SetRow(AppTitleBar, 0);
                    Grid.SetRow(content, 1);

                    Grid.SetRowSpan(AppTitleBar, 1);
                    Grid.SetRowSpan(content, 2);
                }
                else
                {
                    AppTitleBar.Visibility = Visibility.Collapsed;

                    Grid.SetRow(AppTitleBar, 0);
                    Grid.SetRow(content, 0);

                    Grid.SetRowSpan(AppTitleBar, 0);
                    Grid.SetRowSpan(content, 3);
                }
            }
        (DataContext as NewItemViewModel).GetFiles();
        }
        catch (Exception)
        {
            throw;
        }
    }
}
