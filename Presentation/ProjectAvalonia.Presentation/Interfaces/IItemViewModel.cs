using System.Reactive;
using ReactiveUI;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface IItemViewModel
{
    public string Id
    {
        get;
    }

    public IItemGroupViewModel Parent
    {
        get;
    }

    public string ItemPath
    {
        get;
    }

    public string Name
    {
        get;
    }

    public string TemplateName
    {
        get;
    }

    public ReactiveCommand<IItemViewModel, Unit> CommitFileCommand
    {
        get;
    }


    public ReactiveCommand<IItemGroupViewModel, Unit> CreateItemCommand
    {
        get;
    }

    public ReactiveCommand<IItemGroupViewModel, Unit> ExcludeFileCommand
    {
        get;
    }

    public ReactiveCommand<IItemViewModel, Unit> RenameFileCommand
    {
        get;
    }
}