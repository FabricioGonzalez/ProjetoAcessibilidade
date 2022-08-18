using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;

using Infrastructure.WindowsStorageRepository;

using Microsoft.UI.Xaml;

using ProjetoAcessibilidade.Contracts.Services;
using ProjetoAcessibilidade.Services;

using SystemApplication.Services.ProjectDataServices;
using SystemApplication.Services.UIOutputs;

namespace ProjetoAcessibilidade.ViewModels.DialogViewModel;
public class NewItemViewModel : ObservableRecipient
{
    public ObservableCollection<FileTemplates> files = new();
    public ObservableCollection<FileTemplates> Files
    {
        get => files;
        set => SetProperty(ref files, value);
    }

    readonly GetProjectData getAppLocal;
    public NewItemViewModel(GetProjectData local)
    {
        getAppLocal = local;
    }
    public async Task<List<FileTemplates>> GetFiles()
    {
        List<FileTemplates> result = await getAppLocal.GetProjectItens();

        return result;

    }
}
