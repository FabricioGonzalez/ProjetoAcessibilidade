using System;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using Common;
using ProjectAvalonia.Common.Helpers;
using ProjectAvalonia.Features.Project.Services;
using ProjectAvalonia.Features.Project.ViewModels.Dialogs;
using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.ViewModels.Navigation;
using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels;

public partial class ItemViewModel
    : ReactiveObject
        , IItemViewModel
{
    private readonly ItemsService _itemsService;
    private readonly Func<Task> _saveSolution;
    private bool _isEditing;

    [AutoNotify]
    private string _name;

    public ItemViewModel(
        string id
        , string itemPath
        , string name
        , string templateName
        , IItemGroupViewModel parent
        , ItemsService itemsService
        , Func<Task> saveSolution
    )
    {
        ItemPath = itemPath;
        Id = id;
        Name = name;
        TemplateName = templateName;
        Parent = parent;
        _itemsService = itemsService;
        _saveSolution = saveSolution;
        CommitFileCommand = ReactiveCommand.CreateFromTask<IItemGroupViewModel>(CommitFile);
        SelectItemToEditCommand = ReactiveCommand.Create<IItemViewModel>(SelectItemToEdit);
        ExcludeFileCommand =
            ReactiveCommand.Create(() =>
            {
            });
        CanMoveCommand =
            ReactiveCommand.CreateFromTask<IItemGroupViewModel>(async parent =>
            {
                var dialog = new DeleteDialogViewModel(
                    message: "O item seguinte será excluido ao confirmar. Deseja continuar?",
                    title: "Deletar Item",
                    caption: "");

                if ((await RoutableViewModel.NavigateDialogAsync(
                        dialog: dialog,
                        target: NavigationTarget.CompactDialogScreen)).Result.Item2)
                {
                    await MoveItem(parent);
                }
            });

        _ = ExcludeFileCommand.Subscribe();

        RenameFileCommand = ReactiveCommand.Create(() => RenameFile());
    }

    public string Id
    {
        get;
    }

    public IItemGroupViewModel Parent
    {
        get;
        set;
    }

    public string ItemPath
    {
        get;
        set;
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

    public bool InEditing
    {
        get => _isEditing;
        set => this.RaiseAndSetIfChanged(backingField: ref _isEditing, newValue: value);
    }

    public ReactiveCommand<Unit, Unit> ExcludeFileCommand
    {
        get;
        set;
    } = ReactiveCommand.Create(() =>
    {
    });

    public ReactiveCommand<Unit, Unit> RenameFileCommand
    {
        get;
        set;
    }

    public ReactiveCommand<IItemGroupViewModel, Unit> CanMoveCommand
    {
        get;
        set;
    }

    private async Task MoveItem(
        IItemGroupViewModel parent
    )
    {
        if (parent.Items.Any(x => x.Name == Name))
        {
            NotificationHelpers.Show(title: "Erro", message: "O Item Já Existe");

            return;
        }

        _ = Parent.Items.Remove(this);
        Parent = parent;
        Parent.Items.Add(this);

        var oldPath = ItemPath;

        ItemPath = Path.Combine(path1: parent.ItemPath, path2: Path.GetFileName(oldPath));

        /*
        _ = (await _mediator.Send(new MoveFileItemCommand(oldPath, ItemPath), CancellationToken.None))
            .IfFail(error => NotificationHelpers.Show("Erro", error.Message));
            */

        _ = Parent.MoveItemCommand.Execute();
    }

    private void RenameFile() => InEditing = true;

    private void SelectItemToEdit(
        IItemViewModel item
    )
    {
    }

    private async Task CommitFile(
        IItemGroupViewModel item
        , CancellationToken token
    )
    {
        if (ItemPath is { Length: > 0 } path)
        {
            var result = path.Split(Path.DirectorySeparatorChar)[..^1];

            var newPath = string.Join(separator: Path.DirectorySeparatorChar
                , values: result.Append($"{Name}{Constants.AppProjectItemExtension}"));

            _itemsService.RenameFile(oldPath: ItemPath, newPath: newPath);

            ItemPath = newPath;


            await _saveSolution();
        }
    }
}