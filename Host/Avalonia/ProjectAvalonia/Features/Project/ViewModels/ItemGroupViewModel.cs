using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Core.Entities.Solution.ItemsGroup;
using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels;

public class ItemGroupViewModel : ReactiveObject, IItemGroupViewModel
{
    public ItemGroupViewModel(string name, string itemPath)
    {
        Name = name;
        ItemPath = itemPath;

        CommitFolderCommand = ReactiveCommand.CreateFromTask<IItemGroupViewModel>(execute: CommitFolder);
        AddProjectItemCommand = ReactiveCommand.CreateFromTask<IItemGroupViewModel>(execute: AddProjectItem);
        ExcludeFolderCommand = ReactiveCommand.CreateFromTask<IItemGroupViewModel>(execute: ExcludeFolder);
    }

    public string Name
    {
        get;
    }

    public string ItemPath
    {
        get;
    }

    public IEnumerable<IItemViewModel> Items
    {
        get;
        private set;
    }

    public ReactiveCommand<IItemGroupViewModel, Unit> CommitFolderCommand
    {
        get;
    }

    public ReactiveCommand<IItemGroupViewModel, Unit> AddProjectItemCommand
    {
        get;
    }

    public ReactiveCommand<IItemGroupViewModel, Unit> ExcludeFolderCommand
    {
        get;
    }

    public void TransformFrom(List<ItemModel> items) => Items = items.Select(selector: model =>
        new ItemViewModel(id: model.Id, itemPath: model.ItemPath, name: model.Name, templateName: model.TemplateName,
            parent: this));

    private async Task ExcludeFolder(IItemGroupViewModel item)
    {
    }

    private async Task AddProjectItem(IItemGroupViewModel item)
    {
    }

    private async Task CommitFolder(IItemGroupViewModel item)
    {
    }
}