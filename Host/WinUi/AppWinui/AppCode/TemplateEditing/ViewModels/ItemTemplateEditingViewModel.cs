using System.Collections.ObjectModel;

using AppWinui.AppCode.AppUtils.Contracts.ViewModels;
using AppWinui.AppCode.TemplateEditing.Contracts;
using AppWinui.Core.Contracts.Services;
using AppWinui.Core.Models;

using ReactiveUI;

namespace AppWinui.AppCode.TemplateEditing.ViewModels;

public class ItemTemplateEditingViewModel : ReactiveObject
{
    private readonly ISampleDataService _sampleDataService;
    private SampleOrder? _selected;

    public SampleOrder? Selected
    {
        get => _selected;
        set => this.RaiseAndSetIfChanged(ref _selected, value);
    }

    public ObservableCollection<SampleOrder> SampleItems { get; private set; } = new ObservableCollection<SampleOrder>();

    public ItemTemplateEditingViewModel()
    {
        /*_sampleDataService = sampleDataService;*/
    }

}
