using System.Collections.ObjectModel;
using ProjectAvalonia.Presentation.Interfaces;
using ProjetoAcessibilidade.Core.Enuns;
using ReactiveUI;

namespace ProjectAvalonia.Presentation.States.FormItemState;

public class TextItemState : FormItemStateBase
{
    private string? _measurementUnit;

    private string _textData = "";

    private string _topic = "";
    private ObservableCollection<IValidationRuleState> _validationRules = new();

    public TextItemState(
        string topic
        , string textData
        , string? measurementUnit = null
        , AppFormDataType type = AppFormDataType.Texto
        , string id = ""
    )
        : base(type, id)
    {
        Topic = topic;
        TextData = textData;
        MeasurementUnit = measurementUnit;
    }

    public ObservableCollection<IValidationRuleState> ValidationRules
    {
        get => _validationRules;
        set => this.RaiseAndSetIfChanged(ref _validationRules, value);
    }

    public string? MeasurementUnit
    {
        get => _measurementUnit;
        set => this.RaiseAndSetIfChanged(ref _measurementUnit, value);
    }

    public string TextData
    {
        get => _textData;
        set => this.RaiseAndSetIfChanged(ref _textData, value);
    }

    public string Topic
    {
        get => _topic;
        set => this.RaiseAndSetIfChanged(ref _topic, value);
    }
}