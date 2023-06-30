using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Core.Entities.ValidationRules;
using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.Presentation.States.ValidationRulesState;
using ProjectAvalonia.ViewModels;

namespace ProjectAvalonia.Features.TemplateEdit.ViewModels;

[NavigationMetaData(
    Title = "Template Validation",
    Caption = "Valides project items templates",
    Order = 0,
    Category = "Validation",
    Searchable = true,
    Keywords = new[]
    {
        "Templates", "Editing", "Validation"
    },
    NavBarPosition = NavBarPosition.Top,
    NavigationTarget = NavigationTarget.HomeScreen)]
public partial class ItemValidationViewModel : TemplateEditTabViewModelBase, IItemValidationRulesViewModel
{
    [AutoNotify] private ValidationRuleContainerState? _validationItemRules;

    [AutoNotify] private ObservableCollection<ValidationRule> _validationRules = new()
    {
        new ValidationRule
        {
            Rules = new List<RuleSet>
            {
                new()
                {
                    Operation = "Obrigatority",
                    Conditions = new List<Conditions>
                    {
                        new("0", "has", "checked", new List<string> { "Teste", "Teste 2" }, teste =>
                        {
                            return (false, Enumerable.Empty<string>());
                        })
                    }
                }
            },
            Target = new Target()
        }
    };

    public override string? LocalizedTitle
    {
        get;
        protected set;
    }

    public override MenuViewModel? ToolBar => null;
}