namespace ProjectAvalonia.Presentation.Interfaces;

public interface ITextFormItemViewModel : IFormViewModel
{
    public string Topic
    {
        get;
    }

    public string TextData
    {
        get;
    }

    public string MeasurementUnit
    {
        get;
    }
}