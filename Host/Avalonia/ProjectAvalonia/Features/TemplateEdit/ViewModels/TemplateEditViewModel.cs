using ProjectAvalonia.Features.NavBar;

namespace ProjectAvalonia.Features.TemplateEdit.ViewModels;

[NavigationMetaData(
    Title = "Template Edit",
    Caption = "Edit project items templates",
    Order = 0,
    Category = "Templates",
    Searchable = true,
    Keywords = new[]
    {
            "Templates", "Editing"
    },
        NavBarPosition = NavBarPosition.Top,
    NavigationTarget = NavigationTarget.HomeScreen)]
public partial class TemplateEditViewModel : NavBarItemViewModel
{
    [AutoNotify] private int _selectedTab;

    public TemplateEditViewModel()
    {
        _selectedTab = 0;

        SelectionMode = NavBarItemSelectionMode.Button;

        TemplateEditTab = new();

    }
    public TemplateEditTabViewModel TemplateEditTab
    {
        get;
    }
}
