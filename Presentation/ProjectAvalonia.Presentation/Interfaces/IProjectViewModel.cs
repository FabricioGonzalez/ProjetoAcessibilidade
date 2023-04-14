using System.ComponentModel;
using System.Reactive;
using ProjectAvalonia.Presentation.States;
using ReactiveUI;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface IProjectViewModel : INotifyPropertyChanged
{
    public IProjectExplorerViewModel ProjectExplorerViewModel
    {
        get;
    }

    public IProjectEditingViewModel ProjectEditingViewModel
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> OpenProjectCommand
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> CreateProjectCommand
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> PrintProjectCommand
    {
        get;
    }

    public ReactiveCommand<SolutionState, Unit> SaveSolutionCommand
    {
        get;
    }
}