using System.Collections.ObjectModel;
using System.Reactive;
using ProjectAvalonia.Models.ValidationTypes;
using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.Presentation.States.FormItemState;
using ProjectAvalonia.ViewModels;
using ProjectAvalonia.ViewModels.Dialogs.Base;
using ReactiveUI;

namespace ProjectAvalonia.Features.TemplateEdit.ViewModels;

public partial class EditRuleDialogViewModel : DialogViewModelBase<IValidationRuleState>
{
    private ObservableCollection<OptionsItemState> _optionsItems = new();
    [AutoNotify] private OptionsItemState _selectedCheckbox;
    [AutoNotify] private IValidationRuleState _validationRuleState;

    public EditRuleDialogViewModel(
        IValidationRuleState validationRuleState
    )
    {
        SetupCancel(false, true, true);

        ValidationRuleState = validationRuleState;

        NextCommand = ReactiveCommand.Create(
            OnNext);
    }

    public ReactiveCommand<Unit, Unit> AddConditionCommand => ReactiveCommand.Create(() =>
    {
        ValidationRuleState.Conditions.Add(new ConditionState());
    });

    public ReactiveCommand<IConditionState, Unit> ExcludeResultCommand => ReactiveCommand.Create<IConditionState>(
        conditionState =>
        {
            conditionState.Result.Add("");
        });

    public ReactiveCommand<IConditionState, Unit> AddResultCommand => ReactiveCommand.Create<IConditionState>(
        conditionState =>
        {
            conditionState.Result.Add("");
        });

    public ReactiveCommand<IConditionState, Unit> ExcludeConditionCommand => ReactiveCommand.Create<IConditionState>(
        conditionState =>
        {
            ValidationRuleState.Conditions.Remove(conditionState);
        });

    public override string Title
    {
        get;
        protected set;
    } = "Teste";

    public override string? LocalizedTitle
    {
        get;
        protected set;
    }

    public override MenuViewModel? ToolBar
    {
        get;
    } = null;

    public ObservableCollection<OptionsItemState> Options
    {
        get => _optionsItems;
        set => this.RaiseAndSetIfChanged(ref _optionsItems, value);
    }

    public void SetCheckboxItems(
        ObservableCollection<OptionsItemState> checkboxItems
    ) =>
        Options = checkboxItems;

    private void OnNext() => Close(DialogResultKind.Normal, ValidationRuleState);
}