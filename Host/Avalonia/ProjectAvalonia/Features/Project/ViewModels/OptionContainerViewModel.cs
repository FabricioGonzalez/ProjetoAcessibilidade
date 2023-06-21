using System.Collections.ObjectModel;

using ProjectAvalonia.Presentation.Interfaces;

using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels;

public class OptionContainerViewModel : ReactiveObject, IOptionsContainerViewModel
{
    public OptionContainerViewModel(ObservableCollection<IOptionViewModel> options)
    {
        Options = options;
    }

    public ObservableCollection<IOptionViewModel> Options
    {
        get;
    }
}