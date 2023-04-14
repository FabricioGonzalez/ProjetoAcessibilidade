using System.Collections.Generic;
using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.DesignTimeViewModels;

public class DesignTimeProjectEditingViewModel : ReactiveObject, IProjectEditingViewModel
{
    public IEditingItemViewModel SelectedItem
    {
        get;
    } = new DesignTimeEditingItemViewModel();

    public List<IEditingItemViewModel> EditingItems
    {
        get;
    } = new()
    {
        new DesignTimeEditingItemViewModel(),
        new DesignTimeEditingItemViewModel()
    };
}