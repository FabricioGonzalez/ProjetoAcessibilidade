using System.Collections.Generic;
using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.DesignTimeViewModels;

public class DesignTimeOptionContainerViewModel : ReactiveObject, IOptionsContainerViewModel
{
    public List<IOptionViewModel> Options
    {
        get;
    } = new()
    {
        new DesignTimeOptionCheckedItemViewModel(),
        new DesignTimeOptionUnCheckedItemViewModel()
    };
}