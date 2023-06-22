using System.Collections.ObjectModel;

using Core.Entities.Solution.Project.AppItem;

using ProjectAvalonia.Presentation.Interfaces;

using ReactiveUI;

namespace ProjectAvalonia.DesignTimeViewModels;

public class DesignTimeObservationFormItem : ReactiveObject, IObservationFormItemViewModel
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