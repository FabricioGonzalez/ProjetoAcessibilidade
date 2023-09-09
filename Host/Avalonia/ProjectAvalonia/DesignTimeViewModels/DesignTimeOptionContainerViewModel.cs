using System.Collections.ObjectModel;
using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.DesignTimeViewModels;

public class DesignTimeOptionContainerViewModel : ReactiveObject, IOptionsContainerViewModel
{
    public ObservableCollection<IOptionViewModel> Options
    {
        get;
    } = new()
    {
        new DesignTimeOptionCheckedItemViewModel(),
        new DesignTimeOptionUnCheckedItemViewModel()
    };
}