using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Core.Entities.Solution.Project.AppItem;
using Project.Domain.App.Models;
using Project.Domain.Contracts;
using Project.Domain.Project.Commands.ProjectItems;
using Project.Domain.Project.Queries.ProjectItems;
using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Common.Helpers;
using ProjectAvalonia.Features.Project.States;
using ProjectAvalonia.Features.Project.States.FormItemState;
using ProjectAvalonia.Features.Project.States.ProjectItems;
using ProjectAvalonia.Logging;
using ProjectAvalonia.ViewModels;
using ReactiveUI;
using Splat;

namespace ProjectAvalonia.Features.Project.ViewModels;

public partial class ProjectEditingViewModel : ViewModelBase
{
    private readonly ICommandDispatcher? commandDispatcher;

    private readonly IQueryDispatcher? queryDispatcher;
    [AutoNotify] private AppModelState _item;
    [AutoNotify] private ObservableCollection<AppModelState> _items = new();
    [AutoNotify] private int _selectedIndex;
    [AutoNotify] private ItemState _selectedItem;

    public ProjectEditingViewModel()
    {
        queryDispatcher ??= Locator.Current.GetService<IQueryDispatcher>();
        commandDispatcher ??= Locator.Current.GetService<ICommandDispatcher>();

        var canSave = this
            .WhenAnyValue(property1: vm => vm.Item)
            .Select(selector: prop => prop is not null);

        this.WhenAnyValue(property1: vm => vm.SelectedItem)
            .WhereNotNull()
            .SubscribeAsync(onNextAsync: async prop =>
            {
                Item = await SetEditingItem(path: prop.ItemPath);

                var itemIndex = Items.IndexOf(item: Item);

                SelectedIndex = itemIndex != -1 ? itemIndex : 0;
            });

        SaveItemCommand = ReactiveCommand.CreateFromTask<AppModelState>(
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

        CloseItemCommand = ReactiveCommand.Create<AppModelState>(execute: item =>
        {
            Logger.LogDebug(message: item.ItemName);
            Items.Remove(item: item);
        });
    }

    public ReactiveCommand<AppModelState, Unit> SaveItemCommand
    {
        get;
        private set;
    }

    public ReactiveCommand<AppModelState, Unit> CloseItemCommand
    {
        get;
    }

    public ReactiveCommand<ImageContainerItemState, Unit> AddPhotoCommand
    {
        get;
        private set;
    }

    public async Task<AppModelState> SetEditingItem(
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
    }
}