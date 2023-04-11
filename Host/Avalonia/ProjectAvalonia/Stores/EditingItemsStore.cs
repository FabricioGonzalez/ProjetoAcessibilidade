using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Core.Entities.Solution.Project.AppItem;
using DynamicData;
using Project.Domain.Contracts;
using Project.Domain.Project.Queries.ProjectItems;
using ProjectAvalonia.Features.Project.States;
using ProjectAvalonia.Features.Project.States.ProjectItems;
using ReactiveUI;

namespace ProjectAvalonia.Stores;

public partial class EditingItemsStore
{
    private readonly IQueryDispatcher _queryDispatcher;

    private readonly SourceList<ItemState> _selectedItemCollection;
    [AutoNotify] private ItemState _currentSelectedItem;
    [AutoNotify] private AppModelState _item;

    [AutoNotify] private ReadOnlyObservableCollection<ItemState> _selectedItems;

    public EditingItemsStore(IQueryDispatcher queryDispatcher)
    {
        _queryDispatcher = queryDispatcher;

        _selectedItemCollection = new SourceList<ItemState>();
        _selectedItemCollection.Connect()
            .ObserveOn(scheduler: RxApp.MainThreadScheduler)
            .Bind(readOnlyObservableCollection: out _selectedItems)
            .Subscribe();
    }

    public void RemoveEditingItem(ItemState item) => _selectedItemCollection.Remove(item: item);

    public async Task EditItem(ItemState item) =>
        (await _queryDispatcher
            .Dispatch<GetProjectItemContentQuery, Resource<AppItemModel>>(
                query: new GetProjectItemContentQuery(ItemPath: item.ItemPath),
                cancellation: CancellationToken.None))
        .OnSuccess(onSuccessAction: success =>
        {
            if (_selectedItemCollection.Items.All(predicate: i => i.ItemPath != item.ItemPath))
            {
                Item = success?.Data?.ToAppState();

                _selectedItemCollection.Add(item: item);
                _currentSelectedItem = item;
            }
        })
        .OnError(onErrorAction: error =>
        {
        });
}