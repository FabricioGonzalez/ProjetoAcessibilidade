using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;

using ProjetoAcessibilidade.Contracts.ViewModels;

using SystemApplication.Services.ProjectDataServices;
using SystemApplication.Services.UIOutputs;

namespace ProjetoAcessibilidade.ViewModels;
public class TemplateEditViewModel : ObservableRecipient, INavigationAware
{

    readonly GetProjectData getProjectData;

    private FileTemplates _selected;

    public FileTemplates Selected
    {
        get => _selected;
        set
        {
            SetProperty(ref _selected, value);
            ProjectItem = getProjectData.GetItemProject(Selected.Path);
        }
    }

    private ItemModel _projectItem;

    public ItemModel ProjectItem
    {
        get => _projectItem;
        set => SetProperty(ref _projectItem, value);
    }

    public ObservableCollection<FileTemplates> SampleItems { get; private set; } = new ObservableCollection<FileTemplates>();

    public TemplateEditViewModel(GetProjectData getProjectData)
    {
        this.getProjectData = getProjectData;
    }

    public async void OnNavigatedTo(object parameter)
    {
        SampleItems.Clear();

        // TODO: Replace with real data.
        var data = await getProjectData.GetProjectItens();

        foreach (var item in data)
        {
            SampleItems.Add(item);
        }
    }

    public void OnNavigatedFrom()
    {
    }

    public void EnsureItemSelected()
    {
        if (Selected == null)
        {
            if (SampleItems.Count > 0)
                Selected = SampleItems.First();
        }
    }
}
