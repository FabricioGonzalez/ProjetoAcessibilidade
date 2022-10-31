using System.Collections.ObjectModel;

using AppUsecases.Contracts.Usecases;
using AppUsecases.Editing.Entities;

using AppWinui.AppCode.AppUtils.Services;

using Common;

using CommunityToolkit.Mvvm.ComponentModel;

namespace AppWinui.AppCode.Dialogs.ViewModels;
public class NewItemViewModel : ObservableRecipient
{
    private ObservableCollection<FileTemplate> files = new();
    public ObservableCollection<FileTemplate> Files
    {
        get => files;
        set => SetProperty(ref files, value);
    }

    private FileTemplate item = new();
    public FileTemplate Item
    {
        get => item;
        set => item = value;
    }

    readonly InfoBarService infoBarService;
    readonly IQueryUsecase<List<FileTemplate>> getAppLocal;
    public NewItemViewModel(IQueryUsecase<List<FileTemplate>> local, InfoBarService infoBarService)
    {
        getAppLocal = local;
        this.infoBarService = infoBarService;
    }
    public async Task<List<FileTemplate>> GetFiles()
    {
        try
        {
            var  result = await getAppLocal.executeAsync();
            if (result is Resource<List<FileTemplate>>.Error err)
            {
                infoBarService.SetMessageData("Erro", err.Message, Microsoft.UI.Xaml.Controls.InfoBarSeverity.Error);
                return new();

            }
            if (result is Resource<List<FileTemplate>>.Success success)
            {
                Files = new(success.Data);
                return success.Data;
            }

            return new();
        }
        catch (Exception ex)
        {
            infoBarService.SetMessageData(ex.Source, ex.ToString(), Microsoft.UI.Xaml.Controls.InfoBarSeverity.Error);
            return null;
        }

    }
}
