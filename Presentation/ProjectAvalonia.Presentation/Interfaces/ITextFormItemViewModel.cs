namespace ProjectAvalonia.Presentation.Interfaces;

public interface ITextFormItemViewModel : IFormViewModel
{
    public string Topic
    {
        get;
    }
    public string Id
    {
        get;
        set;
    }
    public string TextData
    {
        get; set;
    }

    public string MeasurementUnit
    {
        get;
    }
}