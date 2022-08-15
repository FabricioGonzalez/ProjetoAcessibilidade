using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using Infrastructure.InMemoryRepository;

using ProjetoAcessibilidade.Services;

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

    readonly GetAppLocal getAppLocal;
    public readonly NewItemDialogService newItemDialogService;
    public NewItemViewModel(GetAppLocal local, NewItemDialogService newItemDialog)
    {
        getAppLocal = local;
        newItemDialogService = newItemDialog;
    }
    public NewItemViewModel()
    {

    }

    public void GetFiles()
    {
        Files = new(getAppLocal.getProjectLocalPath());
    }
}
