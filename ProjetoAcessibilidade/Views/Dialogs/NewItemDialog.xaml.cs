using Microsoft.UI.Xaml.Controls;

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
    public NewItemDialog(NewItemViewModel newItemView)
    {
        InitializeComponent();
        this.newItemView = newItemView;

    }

    public async void ContentDialog_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {

        var result = await newItemView.GetFiles();
        newItemView.Files = new(result);
    }

    public void MainButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        if (Lista.SelectedItem is not null)
        {
            FileTemplates file = new()
            {
                Name = TextBox.Text,

                Path = (Lista.SelectedItem as FileTemplates).Path
            };
            item = file;
        }
    }

    private void Lista_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is ListView lista)
        {
            TextBox.Text = (lista.SelectedItem as FileTemplates).Name;
        }
    }
}
