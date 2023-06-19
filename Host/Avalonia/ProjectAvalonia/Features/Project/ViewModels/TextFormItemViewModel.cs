using ProjectAvalonia.Presentation.Interfaces;

using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels;

public class TextFormItemViewModel : ReactiveObject, ITextFormItemViewModel
{
    public TextFormItemViewModel(string topic, string textData, string measurementUnit, string id)
    {
        Topic = topic;
        TextData = textData;
        MeasurementUnit = measurementUnit;
        this.Id = id;
    }
    public string Id
    {
        get; set;
    }
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