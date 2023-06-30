using System.Collections.ObjectModel;
using ReactiveUI;

namespace ProjectAvalonia.Presentation.States.ValidationRulesState;

public class ValidationRuleContainerState : ReactiveObject
{
    private string _targetId;
    private string _targetName;

    private ObservableCollection<ValidationRuleState> _validaitonRules = new();

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

    public ObservableCollection<ValidationRuleState> ValidaitonRules
    {
        get => _validaitonRules;
        set => this.RaiseAndSetIfChanged(ref _validaitonRules, value);
    }
}