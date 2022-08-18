using System;
using System.Threading.Tasks;

using Microsoft.UI.Xaml.Controls;

using ProjetoAcessibilidade.ViewModels.DialogViewModel;
using ProjetoAcessibilidade.Views.Dialogs;

using SystemApplication.Services.UIOutputs;

namespace ProjetoAcessibilidade.Services;
public class NewItemDialogService
{
    readonly NewItemViewModel newItemViewModel;
    public NewItemDialogService(NewItemViewModel newItemViewModel)
    {
        this.newItemViewModel = newItemViewModel;
    }
    private FileTemplates item;
    public async Task<FileTemplates> ShowDialog()
    {
        ContentDialog noWifiDialog = new NewItemDialog(newItemViewModel);

        noWifiDialog.XamlRoot = App.MainWindow.Content.XamlRoot;

        noWifiDialog.PrimaryButtonClick += NoWifiDialog_PrimaryButtonClick;

        noWifiDialog.PrimaryButtonText = "Adicionar";
        noWifiDialog.CloseButtonText = "Cancelar";

        ContentDialogResult result = await noWifiDialog.ShowAsync();

        return item;
    }

    private void NoWifiDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        (sender as NewItemDialog).MainButton_Click(sender, new());

        var result = (sender as NewItemDialog).item;

        item = result;
    }
}
