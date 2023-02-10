using Core.Entities.Solution.ItemsGroup;

using ProjectAvalonia.ViewModels;

namespace ProjectAvalonia.Features.Project.ViewModels;
public partial class ProjectEditingViewModel : ViewModelBase
{
    [AutoNotify] private ItemModel _selectedItem;
    public ProjectEditingViewModel()
    {

    }
}
