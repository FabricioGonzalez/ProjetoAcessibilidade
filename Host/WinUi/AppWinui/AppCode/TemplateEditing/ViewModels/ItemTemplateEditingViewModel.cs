using System.Collections.ObjectModel;

using AppUsecases.Entities;

using AppWinui.Core.Contracts.Services;

using ReactiveUI;

namespace AppWinui.AppCode.TemplateEditing.ViewModels;

public class ItemTemplateEditingViewModel : ReactiveObject
{
    /*private readonly ISampleDataService _sampleDataService;*/
    private AppItemModel? _selected;

    public AppItemModel? Selected
    {
        get => _selected;
        set => this.RaiseAndSetIfChanged(ref _selected, value);
    }

    public ObservableCollection<AppItemModel> SampleItems { get; private set; } = new ObservableCollection<AppItemModel>();

    public ItemTemplateEditingViewModel()
    {
        /*_sampleDataService = sampleDataService;*/
    }

}
