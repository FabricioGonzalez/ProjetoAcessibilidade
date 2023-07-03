using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.ViewModels;
using ProjectAvalonia.ViewModels.Dialogs.Base;

namespace ProjectAvalonia.Features.TemplateEdit.ViewModels;

public partial class EditRuleDialogViewModel : DialogViewModelBase<IValidationRuleState>
{
    [AutoNotify] private IValidationRuleState _validationRuleState;

    public EditRuleDialogViewModel(IValidationRuleState validationRuleState)
    {
        SetupCancel(false, true, true);

        ValidationRuleState = validationRuleState;
    }

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
}