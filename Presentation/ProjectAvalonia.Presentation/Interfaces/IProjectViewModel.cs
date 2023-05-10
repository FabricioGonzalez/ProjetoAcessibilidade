using System.ComponentModel;
using System.Reactive;
using ReactiveUI;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface IProjectViewModel : INotifyPropertyChanged
{
    public bool IsSolutionOpen
    {
        get;
    }

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

    public ReactiveCommand<Unit, Unit> SaveSolutionCommand
    {
        get;
    }
}