using System;
using System.Collections.ObjectModel;

using Core.Entities.Solution.Project.AppItem;

using DynamicData;

using ProjectAvalonia.Presentation.Interfaces;

using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels;

public class ObservationFormItem : ReactiveObject, IObservationFormItemViewModel
{

    public ObservationFormItem()
    {
        SourceItems
            .Connect()
            .Bind(out _observations)
            .Subscribe();
    }
    public string Id
    {
        get;
        set;
    }
    public SourceList<ObservationModel> SourceItems
    {
        get; set;
    } = new();

    public ReadOnlyObservableCollection<ObservationModel> _observations;
    public ReadOnlyObservableCollection<ObservationModel> Observations => _observations;
}