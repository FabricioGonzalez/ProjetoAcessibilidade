using AppUsecases.Editing.Entities;
using AppWinui.AppCode.Dialogs.ViewModels;
using AppWinui.AppCode.Dialogs.Views;

using Microsoft.UI.Xaml.Controls;

namespace AppWinui.AppCode.AppUtils.Services;
public class NewItemDialogService
{
    readonly NewItemViewModel newItemViewModel;

    public static NewItemDialog dialog;

    public NewItemDialogService(NewItemViewModel newItemViewModel)
    {
        this.newItemViewModel = newItemViewModel;
    }
    public async Task<FileTemplate> ShowDialog()
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
