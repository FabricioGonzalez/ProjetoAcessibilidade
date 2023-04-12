using System.Collections.ObjectModel;
using System.Reactive;
using System.Windows.Input;
using Project.Domain.Contracts;
using ProjectAvalonia.Features.Project.States;
using ProjectAvalonia.Features.Project.States.FormItemState;
using ProjectAvalonia.Features.Project.States.ProjectItems;
using ProjectAvalonia.Stores;
using ProjectAvalonia.ViewModels;
using ReactiveUI;
using Splat;

namespace ProjectAvalonia.Features.Project.ViewModels;

public partial class ProjectEditingViewModel : ViewModelBase
{
    private readonly EditingItemsStore _editingItemsStore;
    private readonly ICommandDispatcher? commandDispatcher;

    private readonly IQueryDispatcher? queryDispatcher;

    [AutoNotify] private int _selectedIndex;

    public ProjectEditingViewModel()
    {
        queryDispatcher ??= Locator.Current.GetService<IQueryDispatcher>();
        commandDispatcher ??= Locator.Current.GetService<ICommandDispatcher>();
        _editingItemsStore ??= Locator.Current.GetService<EditingItemsStore>();

        /*SaveItemCommand = ReactiveCommand.CreateFromTask<AppModelState>(
            execute: async appModel =>
            {
                if (appModel is not null)
                {
                    var itemModel = appModel.ToAppModel();

                    await commandDispatcher
                        .Dispatch<SaveProjectItemContentCommand, Resource<Empty>>(
                            command: new SaveProjectItemContentCommand(
                                AppItem: itemModel, ItemPath: SelectedItem.ItemPath),
                            cancellation: CancellationToken.None);
                }
            },
            canExecute: canSave);

        AddPhotoCommand = ReactiveCommand.Create<ImageContainerItemState>(
            execute: async imageContainer =>
            {
                var file = await FileDialogHelper.ShowOpenFileDialogAsync(title: "Obter Image"
                    , filterExtTypes: new[] { "Images/*", "png", "jpeg" });

                if (!string.IsNullOrWhiteSpace(value: file))
                {
                    imageContainer.ImagesItems.Add(item: new ImageItemState { ImagePath = file });
                }
            },
            canExecute: canSave);
*/
        CloseItemCommand = ReactiveCommand.Create<ItemState>(execute: item =>
        {
            _editingItemsStore.RemoveEditingItem(item: item);
        });
    }

    public ItemState SelectedItem => _editingItemsStore.CurrentSelectedItem;
    public AppModelState EditingItem => _editingItemsStore.Item;

    public ReadOnlyObservableCollection<ItemState> Items => _editingItemsStore.SelectedItems;

    public ReactiveCommand<AppModelState, Unit> SaveItemCommand
    {
        get;
    }

    public ICommand CloseItemCommand
    {
        get;
    }

    public ReactiveCommand<ImageContainerItemState, Unit> AddPhotoCommand
    {
        get;
    }

    /*public async Task<AppModelState> SetEditingItem(
        string path
    )
    {
        var result = await queryDispatcher
            .Dispatch<GetProjectItemContentQuery, Resource<AppItemModel>>(
                query: new GetProjectItemContentQuery(ItemPath: path),
                cancellation: CancellationToken.None);

        if (result is Resource<AppItemModel>.Error err)
        {
            return null;
        }

        if (result is Resource<AppItemModel>.Success success)
        {
            var res = success.Data.ToAppState();

            var repitedItems = Items.Any(predicate: i => i.Id == res.Id);

            if (!repitedItems)
            {
                Items.Add(item: res);
            }

            return Items.FirstOrDefault(predicate: i => i.Id == res.Id);
        }

        return null;
    }*/
}