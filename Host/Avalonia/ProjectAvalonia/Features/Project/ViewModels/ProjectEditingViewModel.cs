using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

using Common;

using Core.Entities.Solution.Project.AppItem;

using Project.Domain.Contracts;
using Project.Domain.Project.Commands.ProjectItemCommands.SaveCommands;
using Project.Domain.Project.Queries.GetProjectItemContent;

using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Common.Helpers;
using ProjectAvalonia.Features.Project.States;
using ProjectAvalonia.Features.Project.States.FormItemState;
using ProjectAvalonia.Features.Project.States.ProjectItems;
using ProjectAvalonia.Logging;
using ProjectAvalonia.ViewModels;

using ReactiveUI;

using Splat;

using unit = MediatR.Unit;

namespace ProjectAvalonia.Features.Project.ViewModels;
public partial class ProjectEditingViewModel : ViewModelBase
{
    [AutoNotify] private ItemState _selectedItem;
    [AutoNotify] private AppModelState _item;
    [AutoNotify] private int _selectedIndex;
    [AutoNotify] private ObservableCollection<AppModelState> _items = new();

    private readonly IQueryDispatcher? queryDispatcher;
    private readonly ICommandDispatcher? commandDispatcher;

    public ProjectEditingViewModel()
    {
        queryDispatcher ??= Locator.Current.GetService<IQueryDispatcher>();
        commandDispatcher ??= Locator.Current.GetService<ICommandDispatcher>();

        var canSave = this
           .WhenAnyValue(vm => vm.Item)
           .Select(prop => prop is not null);

        this.WhenAnyValue(vm => vm.SelectedItem)
            .WhereNotNull()
            .SubscribeAsync(async prop =>
            {
                Item = await SetEditingItem(prop.ItemPath);

                var itemIndex = Items.IndexOf(Item);

                SelectedIndex = itemIndex != -1 ? itemIndex : 0;
            });

        SaveItemCommand = ReactiveCommand.CreateFromTask<AppModelState>(
       execute: async (appModel) =>
       {
           if (appModel is not null)
           {
               AppItemModel itemModel = appModel.ToAppModel();

               await commandDispatcher
                 .Dispatch<SaveProjectItemContentCommand, Resource<unit>>(
                     new(
                         itemModel, SelectedItem.ItemPath),
                     CancellationToken.None);

           }
       },
       canExecute: canSave);

        AddPhotoCommand = ReactiveCommand.Create<ImageContainerItemState>(
     execute: async (imageContainer) =>
     {
         var file = await FileDialogHelper.ShowOpenFileDialogAsync("Obter Image", filterExtTypes: new string[] { "Images/*", "png", "jpeg" });

         if (!string.IsNullOrWhiteSpace(file))
         {
             imageContainer.ImagesItems.Add(new() { ImagePath = file });
         }
     },
     canExecute: canSave);

        CloseItemCommand = ReactiveCommand.Create<AppModelState>(execute: (item) =>
        {
            Logger.LogDebug(item.ItemName);
            Items.Remove(item);
        });
    }

    public ReactiveCommand<AppModelState, Unit> SaveItemCommand
    {
        get; private set;
    }
    public ReactiveCommand<AppModelState, Unit> CloseItemCommand
    {
        get;
    }

    public ReactiveCommand<ImageContainerItemState, Unit> AddPhotoCommand
    {
        get; private set;
    }
    public async Task<AppModelState> SetEditingItem(string path)
    {
        var result = await queryDispatcher
            .Dispatch<GetProjectItemContentQuery, Resource<AppItemModel>>(
            new(path),
            CancellationToken.None);

        if (result is Resource<AppItemModel>.Error err)
        {
            return null;
        }

        if (result is Resource<AppItemModel>.Success success)
        {
            var res = success.Data.ToAppState();

            var repitedItems = Items.Any(i => i.Id == res.Id);

            if (!repitedItems)
                Items.Add(res);

            return Items.FirstOrDefault(i => i.Id == res.Id);

        }
        return null;
    }
}
