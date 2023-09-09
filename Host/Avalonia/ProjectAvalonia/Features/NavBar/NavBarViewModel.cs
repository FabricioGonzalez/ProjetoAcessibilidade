using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using ProjectAvalonia.Common.ViewModels;
using ProjectAvalonia.ViewModels.Navigation;

namespace ProjectAvalonia.Features.NavBar;

/// <summary>
///     The ViewModel that represents the structure of the sidebar.
/// </summary>
public class NavBarViewModel : ViewModelBase
{
    public NavBarViewModel()
    {
        TopItems = new ObservableCollection<NavBarItemViewModel>();
        BottomItems = new ObservableCollection<NavBarItemViewModel>();

        SetDefaultSelection();
    }

    public ObservableCollection<NavBarItemViewModel> TopItems
    {
        get;
    }

    public ObservableCollection<NavBarItemViewModel> BottomItems
    {
        get;
    }

    private void SetDefaultSelection()
    {
    }

    public async Task InitialiseAsync()
    {
        var topItems = NavigationManager.MetaData.Where(x => x.NavBarPosition == NavBarPosition.Top);

        var bottomItems = NavigationManager.MetaData.Where(x => x.NavBarPosition == NavBarPosition.Bottom);

        foreach (var item in topItems)
        {
            var viewModel = await NavigationManager.MaterialiseViewModelAsync(item);

            viewModel?.SetTitle(item.LocalizedTitle);

            if (viewModel is NavBarItemViewModel navBarItem)
            {
                TopItems.Add(navBarItem);
            }
        }

        foreach (var item in bottomItems)
        {
            var viewModel = await NavigationManager.MaterialiseViewModelAsync(item);

            viewModel?.SetTitle(item.LocalizedTitle);

            if (viewModel is NavBarItemViewModel navBarItem)
            {
                BottomItems.Add(navBarItem);
            }
        }
    }
}