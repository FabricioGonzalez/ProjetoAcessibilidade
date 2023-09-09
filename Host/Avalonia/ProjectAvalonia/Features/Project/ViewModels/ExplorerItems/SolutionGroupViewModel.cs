using System.Collections.ObjectModel;
using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels;

public class SolutionGroupViewModel
    : ReactiveObject
        , ISolutionGroupViewModel
{
    public ObservableCollection<ISolutionLocationItem> LocationItems
    {
        get;
        set;
    }

    public IItemViewModel ConclusionItem
    {
        get;
        set;
    }

    public IItemViewModel SolutionItem
    {
        get;
        set;
    }
}