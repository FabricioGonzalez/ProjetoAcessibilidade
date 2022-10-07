using System.Collections.ObjectModel;

using AppUsecases.Contracts.Usecases;
using AppUsecases.Entities;
using AppUsecases.Entities.FileTemplate;

using AppWinui.AppCode.AppUtils.Contracts.ViewModels;

using Common;

using ReactiveUI;

namespace AppWinui.AppCode.TemplateEditing.ViewModels;

public class TemplateEditViewModel : ReactiveObject, INavigationAware
{
    readonly IQueryUsecase<List<FileTemplate>> getProjectData;
    /*readonly IQueryUsecase<List<FileTemplate>> getProjectTemplates;*/
    readonly IQueryUsecase<string, AppItemModel> GetProjectItemContentUsecase;

    private FileTemplate _selected;

    public FileTemplate Selected
    {
        get => _selected;
        set
        {
            this.RaiseAndSetIfChanged(ref _selected, value);
            App.MainWindow.DispatcherQueue.TryEnqueue(async () =>
            {

                ProjectItem = await GetProjectItemTemplateContent(Selected.Path);
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

    public TemplateEditViewModel(IQueryUsecase<List<FileTemplate>> getProjectData, IQueryUsecase<string, AppItemModel> appContentTemplate)
    {
        this.getProjectData = getProjectData;
        GetProjectItemContentUsecase = appContentTemplate;
    }

    private async Task<dynamic> GetProjectItemTemplateContent(string path)
    {
        var result = await GetProjectItemContentUsecase.executeAsync(path);
        return result switch
        {
            Resource<AppItemModel>.Success item => item.Data,
            Resource<AppItemModel>.Error item => item.Message,
            Resource<AppItemModel>.IsLoading item => item.isLoading,
            _ => null,
        };

    }


    public async void OnNavigatedTo(object parameter)
    {
        SampleItems.Clear();

        // TODO: Replace with real data.
        dynamic data = await getProjectData.executeAsync() switch
        {
            Resource<List<FileTemplate>>.Success item => item.Data,
            Resource<List<FileTemplate>>.Error item => item.Message,
            Resource<List<FileTemplate>>.IsLoading item => item.isLoading,
            _ => null,
        };
        if (data is not null)
            foreach (var item in data )
            {
                SampleItems.Add(item as FileTemplate);
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
