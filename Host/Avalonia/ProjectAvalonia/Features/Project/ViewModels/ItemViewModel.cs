using System.Reactive;
using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels;

public class ItemViewModel : IItemViewModel
{
    public ItemViewModel(string id, string itemPath, string name, string templateName, IItemGroupViewModel parent)
    {
        ItemPath = itemPath;
        Id = id;
        Name = name;
        TemplateName = templateName;
        Parent = parent;
    }

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