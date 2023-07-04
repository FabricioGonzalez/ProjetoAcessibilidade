using System.Collections.ObjectModel;
using System.Reactive;
using Core.Entities.Solution.Project.AppItem;
using ReactiveUI;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface IObservationFormItemViewModel : IFormViewModel
{
    public ReadOnlyObservableCollection<ObservationModel> Observations
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> AddObservationCommand
    {
        get;
    }

    public string Id
    {
        get;
        set;
    }
}