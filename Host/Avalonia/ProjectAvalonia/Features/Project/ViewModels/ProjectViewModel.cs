using ProjectAvalonia.Features.NavBar;

namespace ProjectAvalonia.Features.Project.ViewModels;

[NavigationMetaData(
    Title = "Project",
    Caption = "Create and edit projects",
    Order = 0,
    Category = "Project",
    Keywords = new[]
    {
            "Project"
    },
        NavBarPosition = NavBarPosition.Top,
    NavigationTarget = NavigationTarget.HomeScreen)]
public partial class ProjectViewModel : NavBarItemViewModel
{
    public ProjectViewModel()
    {
        SetupCancel(enableCancel: false, enableCancelOnEscape: true, enableCancelOnPressed: true);

        SelectionMode = NavBarItemSelectionMode.Button;
    }
}
