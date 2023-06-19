namespace ProjectAvalonia.Presentation.Interfaces;

public interface IObservationFormItemViewModel : IFormViewModel
{
    public string Observation
    {
        get; set;
    }
    public string Id
    {
        get;
        set;
    }
}