using System.Collections.ObjectModel;

using Core.Entities.Solution.Project.AppItem;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface IObservationFormItemViewModel : IFormViewModel
{
    public ReadOnlyObservableCollection<ObservationModel> Observations
    {
        get;
    }
    public string Id
    {
        get;
        set;
    }
}