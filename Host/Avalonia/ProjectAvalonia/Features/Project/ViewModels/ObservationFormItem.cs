using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels;

public class ObservationFormItem : ReactiveObject, IObservationFormItemViewModel
{
    public ObservationFormItem(string observation)
    {
        Observation = observation;
    }

    public string Observation
    {
        get;
    }
}