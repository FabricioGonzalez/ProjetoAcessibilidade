﻿using ProjectAvalonia.Presentation.Interfaces;
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
    public override string? LocalizedTitle
    {
        get;
        protected set;
    }
    [AutoNotify] ValidationRuleContainerState _validationItemRules;
    public override MenuViewModel? ToolBar => null;
}