using System.Collections.ObjectModel;
using System.Reactive;

using ProjectAvalonia.Presentation.Interfaces;

using ReactiveUI;

namespace ProjectAvalonia.DesignTimeViewModels;

public class DesignTimeProjectEditingViewModel
    : ReactiveObject
        , IProjectEditingViewModel
{
    public IEditingItemViewModel SelectedItem
    {
        get;
    } = new DesignTimeEditingItemViewModel();

    public ReadOnlyObservableCollection<IEditingItemViewModel> EditingItems
    {
        get;
    }/* = new()
    {
        new DesignTimeEditingItemViewModel(), new DesignTimeEditingItemViewModel()
    };*/

    public ReactiveCommand<IItemViewModel, Unit> AddItemToEdit
    {
        get;
    }
}