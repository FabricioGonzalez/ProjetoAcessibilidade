using System.Collections.Generic;

using Core.Entities.ValidationRules;

using ProjectAvalonia.Presentation.Interfaces;

using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels;

public partial class TextFormItemViewModel : ReactiveObject, ITextFormItemViewModel
{
    public TextFormItemViewModel(string topic, string textData, string measurementUnit, string id, IEnumerable<ValidationRule> rules)
    {
        Topic = topic;
        TextData = textData;
        MeasurementUnit = measurementUnit;
        Id = id;
        Rules = rules;
    }
    public string Id
    {
        get; set;
    }
    public IEnumerable<ValidationRule> Rules
    {
        get; set;
    }
    public string Topic
    {
        get;
    }
    [AutoNotify] private string _textData;

    public string MeasurementUnit
    {
        get;
    }
}