using Microsoft.UI.Xaml.Controls;

using ProjetoAcessibilidade.Services;
using ProjetoAcessibilidade.ViewModels.DialogViewModel;

using SystemApplication.Services.UIOutputs;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ProjetoAcessibilidade.Views.Dialogs;
/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class NewItemDialog : ContentDialog
{
    public NewItemViewModel newItemView
    {
        get; set;
    }

    public FileTemplates item;
    public NewItemDialog()
    {
        InitializeComponent();
        newItemView = App.GetService<NewItemViewModel>();

    }

    public async void ContentDialog_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var result = await newItemView.GetFiles();
        if (result != null)
        {
            newItemView.Item = null;
            TextBox.Text = string.Empty;
            newItemView.Files = new(result);
        }
    }

    private void Lista_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is ListView lista)
        {
            if (lista.SelectedItem is not null)
                TextBox.Text = (lista.SelectedItem as FileTemplates).Name;
        }
    }

    private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        if (Lista.SelectedItem is not null)
        {
            FileTemplates file = new()
            {
                Name = TextBox.Text,

                Path = (Lista.SelectedItem as FileTemplates).Path
            };
            newItemView.Item = file;
        }
    }
}
