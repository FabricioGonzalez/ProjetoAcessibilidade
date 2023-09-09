using System.Collections.ObjectModel;
using System.Reactive;
using ProjectAvalonia.Presentation.States.FormItemState;
using ReactiveUI;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface IObservationFormItemViewModel : IFormViewModel
{
    public ReadOnlyObservableCollection<ObservationState> Observations
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> AddObservationCommand
    {
        get;
    }

    public ReactiveCommand<ObservationState, Unit> RemoveObservationCommand
    {
        get;
    }

    public string Id
    {
        get;
        set;
    }
}