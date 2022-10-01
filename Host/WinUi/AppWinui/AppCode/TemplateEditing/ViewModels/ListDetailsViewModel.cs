using System.Collections.ObjectModel;

using AppUsecases.Contracts.Usecases;
using AppUsecases.Entities;

using AppWinui.AppCode.AppUtils.Contracts.ViewModels;
using AppWinui.Core.Contracts.Services;
using AppWinui.Core.Models;

using LocalRepository.FileRepository.Repository.InternalAppFiles;

using ReactiveUI;

namespace AppWinui.AppCode.TemplateEditing.ViewModels;

public class ListDetailsViewModel : ReactiveObject, INavigationAware
{

    readonly IQueryUsecase<List<FileTemplate>> getProjectData;
    readonly IQueryUsecase<List<FileTemplate>> getProjectTemplates;

    private FileTemplate _selected;

    public FileTemplate Selected
    {
        get => _selected;
        set
        {
            this.RaiseAndSetIfChanged(ref _selected, value);
            App.MainWindow.DispatcherQueue.TryEnqueue(async () =>
            {
                ProjectItem = await getProjectData.executeAsync(Selected.Path);
            });
        }
    }

    private AppItemModel _projectItem;

    public AppItemModel ProjectItem
    {
        get => _projectItem;
        set => this.RaiseAndSetIfChanged(ref _projectItem, value);
    }
    public ObservableCollection<FileTemplate> SampleItems { get; private set; } = new ObservableCollection<FileTemplate>();

    public TemplateEditViewModel(IQueryUsecase<List<FileTemplate>> getProjectData)
    {
        this.getProjectData = getProjectData;
    }

    public async void OnNavigatedTo(object parameter)
    {
        SampleItems.Clear();

        // TODO: Replace with real data.
        var data = await getProjectData.GetProjectItens();
        if (data is not null)
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
