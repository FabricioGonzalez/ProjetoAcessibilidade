using System.Collections.ObjectModel;
using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.Models.ValidationTypes;

public class ValidationRuleContainerState
    : ReactiveObject
        , IValidationRuleContainerState
{
    private string _targetId;
    private string _targetName;

    private ObservableCollection<IValidationRuleState> _validaitonRules = new();

    public ObservableCollection<IValidationRuleState> ValidaitonRules
    {
        get => _validaitonRules;
        set => this.RaiseAndSetIfChanged(ref _validaitonRules, value);
    }

    public string TargetContainerId
    {
        get => _targetId;
        set => this.RaiseAndSetIfChanged(ref _targetId, value);
    }

    public string TargetContainerName
    {
        get => _targetName;
        set => this.RaiseAndSetIfChanged(ref _targetName, value);
    }
}