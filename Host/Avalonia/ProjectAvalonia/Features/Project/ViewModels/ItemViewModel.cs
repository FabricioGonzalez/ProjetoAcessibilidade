using System;
using System.Linq;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;

using DynamicData;

using ProjectAvalonia.Common.Helpers;
using ProjectAvalonia.Features.Project.ViewModels.Dialogs;
using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.ViewModels.Navigation;

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
        CanMoveCommand =
            ReactiveCommand.CreateFromTask< IItemGroupViewModel>(execute: async(parent) =>
            {
                var dialog = new DeleteDialogViewModel(
                        message: "O item seguinte será excluido ao confirmar. Deseja continuar?",
                        title: "Deletar Item",
                        caption: "");

                if ((await RoutableViewModel.NavigateDialogAsync(
                        dialog: dialog,
                        target: NavigationTarget.CompactDialogScreen)).Result)
                {
                    MoveItem(parent);
                }
            });

        ExcludeFileCommand.Subscribe();

        RenameFileCommand = ReactiveCommand.CreateFromTask<IItemViewModel>(execute: RenameFile);
    }

    private void MoveItem(IItemGroupViewModel parent)
    {
        if(parent.Items.Any(x => x.Name == Name))
        {
            NotificationHelpers.Show("Erro", "O Item Já Existe");

            return;
        }
        Parent.Items.Remove(this);
        Parent = parent;
        Parent.Items.Add(this);

        Parent.MoveItemCommand.Execute();
    }
    public string Id
    {
        get;
    }

    public IItemGroupViewModel Parent
    {
        get; private set;
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
    public ReactiveCommand<IItemGroupViewModel, Unit> CanMoveCommand
    {
        get;
        set;
    }

    private Task RenameFile(IItemViewModel item, CancellationToken token) => Task.CompletedTask;

    private Task SelectItemToEdit(IItemViewModel item, CancellationToken token) => Task.CompletedTask;

    private Task CommitFile(IItemGroupViewModel item, CancellationToken token) => Task.CompletedTask;
}