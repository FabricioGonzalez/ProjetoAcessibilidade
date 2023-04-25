using System.Collections.ObjectModel;
using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.DesignTimeViewModels;

public class DesignTimeProjectEditingViewModel : ReactiveObject, IProjectEditingViewModel
{
    public IEditingItemViewModel SelectedItem
    {
        get;
    } = new DesignTimeEditingItemViewModel();

    public ObservableCollection<IEditingItemViewModel> EditingItems
    {
        get;
    } = new()
    {
        new DesignTimeEditingItemViewModel(),
        new DesignTimeEditingItemViewModel()
    };
}