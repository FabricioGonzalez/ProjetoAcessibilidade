using System;
using System.Collections.ObjectModel;
using System.Reactive;
using DynamicData;
using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.Presentation.States.FormItemState;
using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels;

public class ObservationFormItem
    : ReactiveObject
        , IObservationFormItemViewModel
{
    public ReadOnlyObservableCollection<ObservationState> _observations;

    public ObservationFormItem(
        SourceList<ObservationState> sourceItems
    )
    {
        SourceItems = sourceItems;

        SourceItems
            .Connect()
            .Bind(out _observations)
            .Subscribe();
    }

    public SourceList<ObservationState> SourceItems
    {
        get;
        set;
    }

    public ReactiveCommand<Unit, Unit> AddObservationCommand =>
        ReactiveCommand.Create(() => SourceItems.Add(new ObservationState()));

    public ReactiveCommand<ObservationState, Unit> RemoveObservationCommand =>
        ReactiveCommand.Create<ObservationState>(it => SourceItems.Remove(it));

    public string Id
    {
        get;
        set;
    }

    public ReadOnlyObservableCollection<ObservationState> Observations => _observations;
}