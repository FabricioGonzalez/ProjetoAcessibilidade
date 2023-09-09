using System.Collections.ObjectModel;
using ProjectAvalonia.Presentation.Enums;
using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.Presentation.States.FormItemState;

public class TextItemState
    : FormItemStateBase
{
    private string? _measurementUnit;

    private string _textData = "";

    private string _topic = "";
    private ObservableCollection<IValidationRuleContainerState> _validationRules = new();

    public TextItemState(
        string topic
        , string textData
        , string? measurementUnit = null
        , AppFormDataType type = AppFormDataType.Text
        , string id = ""
    )
        : base(type: type, id: id)
    {
        Topic = topic;
        TextData = textData;
        MeasurementUnit = measurementUnit;
    }

    public ObservableCollection<IValidationRuleContainerState> ValidationRules
    {
        get => _validationRules;
        set => this.RaiseAndSetIfChanged(backingField: ref _validationRules, newValue: value);
    }

    public string? MeasurementUnit
    {
        get => _measurementUnit;
        set => this.RaiseAndSetIfChanged(backingField: ref _measurementUnit, newValue: value);
    }

    public string TextData
    {
        get => _textData;
        set => this.RaiseAndSetIfChanged(backingField: ref _textData, newValue: value);
    }

    public string Topic
    {
        get => _topic;
        set => this.RaiseAndSetIfChanged(backingField: ref _topic, newValue: value);
    }
}