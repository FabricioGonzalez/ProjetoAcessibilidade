using System.Reactive;

using ProjectAvalonia.ViewModels.Dialogs.Base;

namespace ProjectAvalonia.Features.TemplateEdit.ViewModels;
[NavigationMetaData(
    Title = "Template Validation",
    Caption = "Valides project items templates",
    Order = 0,
    Category = "Validation",
    Searchable = true,
    Keywords = new[]
    {
            "Templates", "Editing","Validation"
    },
        NavBarPosition = NavBarPosition.Top,
    NavigationTarget = NavigationTarget.HomeScreen)]

public partial class ItemValidationViewModel : DialogViewModelBase<Unit>
{
}
