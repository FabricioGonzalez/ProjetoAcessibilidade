using ProjectAvalonia.Features.NavBar;

namespace ProjectAvalonia.ViewModels.Dialogs.Base;

/// <summary>
/// CommonBase class.
/// </summary>
public abstract partial class DialogViewModelBase : NavBarItemViewModel
{
    [AutoNotify] private bool _isDialogOpen;
}
