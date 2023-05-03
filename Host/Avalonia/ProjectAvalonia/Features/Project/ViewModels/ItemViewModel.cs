using System;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels;

public class ItemViewModel : ReactiveObject, IItemViewModel
{
    public ItemViewModel(string id, string itemPath, string name, string templateName, IItemGroupViewModel parent)
    {
        ItemPath = itemPath;
        Id = id;
        Name = name;
        TemplateName = templateName;
        Parent = parent;

        CommitFileCommand = ReactiveCommand.CreateFromTask<IItemGroupViewModel>(execute: CommitFile);
        SelectItemToEditCommand = ReactiveCommand.CreateFromTask<IItemViewModel>(execute: SelectItemToEdit);
        ExcludeFileCommand =
            ReactiveCommand.Create(execute: () =>
            {
            });

        ExcludeFileCommand.Subscribe();

        RenameFileCommand = ReactiveCommand.CreateFromTask<IItemViewModel>(execute: RenameFile);
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

    public ReactiveCommand<IItemGroupViewModel, Unit> CommitFileCommand
    {
        get;
    }

    public ReactiveCommand<IItemViewModel, Unit> SelectItemToEditCommand
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> ExcludeFileCommand
    {
        get;
        set;
    } = ReactiveCommand.Create(execute: () =>
    {
    });

    public ReactiveCommand<IItemViewModel, Unit> RenameFileCommand
    {
        get;
        set;
    }

    private Task RenameFile(IItemViewModel item, CancellationToken token) => Task.CompletedTask;

    private Task SelectItemToEdit(IItemViewModel item, CancellationToken token) => Task.CompletedTask;

    private Task CommitFile(IItemGroupViewModel item, CancellationToken token) => Task.CompletedTask;
}