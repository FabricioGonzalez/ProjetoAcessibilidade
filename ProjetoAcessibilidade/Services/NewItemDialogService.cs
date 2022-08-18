using System;
using System.Threading.Tasks;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using ProjetoAcessibilidade.ViewModels.DialogViewModel;
using ProjetoAcessibilidade.Views.Dialogs;

using SystemApplication.Services.UIOutputs;

namespace ProjetoAcessibilidade.Services;
public class NewItemDialogService
{
    readonly NewItemViewModel newItemViewModel;

    public static NewItemDialog dialog;


    public NewItemDialogService(NewItemViewModel newItemViewModel)
    {
        this.newItemViewModel = newItemViewModel;
    }
    public async Task<FileTemplates> ShowDialog()
    {
        var r = App.GetService<InfoBarService>();

        try
        {
            ContentDialogResult result = await dialog.ShowAsync(ContentDialogPlacement.InPlace);

            if (ContentDialogResult.Primary == result)
            {
                return newItemViewModel.Item;
            }

            return null;
        }
        catch (Exception ex)
        {
            r.SetMessageData("Erro", ex.ToString(), InfoBarSeverity.Error);
            return null;
        }
    }
}
