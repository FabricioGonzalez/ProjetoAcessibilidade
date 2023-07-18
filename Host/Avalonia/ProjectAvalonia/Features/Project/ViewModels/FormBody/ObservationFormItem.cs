using System;
using System.Collections.ObjectModel;
using System.Reactive;
using Core.Entities.Solution.Project.AppItem;
using DynamicData;
using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels;

public class ObservationFormItem
    : ReactiveObject
        , IObservationFormItemViewModel
{
    public ReadOnlyObservableCollection<ObservationModel> _observations;

    public ObservationFormItem()
    {
        SourceItems
            .Connect()
            .Bind(out _observations)
            .Subscribe();
    }

    public SourceList<ObservationModel> SourceItems
    {
        get;
        set;
    } = new();

    public ReactiveCommand<Unit, Unit> AddObservationCommand =>
        ReactiveCommand.Create(() => SourceItems.Add(new ObservationModel()));

    public ReactiveCommand<ObservationModel, Unit> RemoveObservationCommand =>
        ReactiveCommand.Create<ObservationModel>(it => SourceItems.Remove(it));

    public string Id
    {
        get;
        set;
    }

    public ReadOnlyObservableCollection<ObservationModel> Observations => _observations;
}