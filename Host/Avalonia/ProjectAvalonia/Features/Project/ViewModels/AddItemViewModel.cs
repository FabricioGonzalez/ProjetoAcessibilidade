using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Common;
using Core.Entities.Solution.Explorer;
using DynamicData.Binding;
using Project.Domain.App.Queries.Templates;
using Project.Domain.Contracts;
using ProjectAvalonia.Features.Project.States.ProjectItems;
using ProjectAvalonia.ViewModels.Dialogs.Base;
using ReactiveUI;
using Splat;

namespace ProjectAvalonia.Features.Project.ViewModels;

[NavigationMetaData(Title = "Adicionar item")]
public partial class AddItemViewModel : DialogViewModelBase<ItemState>
{
    private readonly IQueryDispatcher queryDispatcher;

    [AutoNotify]
    private ItemState _item;

    [AutoNotify]
    private string _itemName = "";

    [AutoNotify]
    private ObservableCollectionExtended<ItemState> _items;

    public AddItemViewModel()
    {
        SetupCancel(enableCancel: true, enableCancelOnEscape: true, enableCancelOnPressed: true);

        queryDispatcher ??= Locator.Current.GetService<IQueryDispatcher>();

        this.WhenAnyValue(property1: vm => vm.Item)
            .WhereNotNull()
            .Subscribe(onNext: item =>
            {
                if (string.IsNullOrWhiteSpace(value: ItemName))
                {
                    ItemName = Item.TemplateName;
                }
            });

        NextCommand = ReactiveCommand.Create(
            execute: OnNext,
            canExecute: this.WhenAnyValue(property1: x => x.Item)
                .Select(selector: prop => prop is not null)
                .ObserveOn(scheduler: RxApp.MainThreadScheduler));


        Dispatcher.UIThread.Post(action: async () =>
        {
            await GetItems();
        });
    }

    public ReactiveCommand<Unit, Unit> CloseDialogCommand
    {
        get;
        set;
    }

    public async Task GetItems()
    {
        var result = await queryDispatcher
            .Dispatch<GetAllTemplatesQuery, Resource<List<ExplorerItem>>>(query: new GetAllTemplatesQuery(),
                cancellation: CancellationToken.None);

        result
            .OnSuccess(onSuccessAction: success =>
            {
                Items = new ObservableCollectionExtended<ItemState>(
                    collection: success
                        ?.Data
                        ?.Select(selector: item => new ItemState
                        {
                            Id = Guid.NewGuid().ToString(), ItemPath = item.Path, Name = "", TemplateName = item.Name
                        }) ?? Enumerable.Empty<ItemState>());
            })
            .OnError(onErrorAction: error =>
            {
            })
            ;
    }

    private void OnNext()
    {
        Item.Name = ItemName;
        Close(kind: DialogResultKind.Normal, result: Item);
    }
}