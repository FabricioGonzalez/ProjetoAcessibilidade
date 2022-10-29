using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;

using ProjetoAcessibilidade.Services;

namespace ProjetoAcessibilidade.ViewModels.DialogViewModel;
public class NewItemViewModel : ObservableRecipient
{
    private ObservableCollection<FileTemplates> files = new();
    public ObservableCollection<FileTemplates> Files
    {
        get => files;
        set => SetProperty(ref files, value);
    }
    
    private FileTemplates item = new();
    public FileTemplates Item
    {
        get => item;
        set => item = value;
    }

    readonly InfoBarService infoBarService;
    readonly GetProjectData getAppLocal;
    public NewItemViewModel(GetProjectData local, InfoBarService infoBarService)
    {
        getAppLocal = local;
        this.infoBarService = infoBarService;
    }
    public async Task<List<FileTemplates>> GetFiles()
    {
        try
        {
            List<FileTemplates> result = await getAppLocal.GetProjectItens();
            return result;
        }
        catch (Exception ex)
        {
            infoBarService.SetMessageData(ex.Source, ex.ToString(), Microsoft.UI.Xaml.Controls.InfoBarSeverity.Error);
            return null;
        }

    }
}
