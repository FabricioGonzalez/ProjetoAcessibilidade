namespace ProjectAvalonia.Stores;

public class EditingItemsStore
{
    /*private readonly IMediator _queryDispatcher;

    private readonly SourceList<ItemState> _selectedItemCollection;
    [AutoNotify] private ItemState _currentSelectedItem;
    [AutoNotify] private AppModelState _item;

    [AutoNotify] private ReadOnlyObservableCollection<ItemState> _selectedItems;

    public EditingItemsStore(IMediator queryDispatcher)
    {
        _queryDispatcher = queryDispatcher;

        _selectedItemCollection = new SourceList<ItemState>();
        _selectedItemCollection.Connect()
            .ObserveOn(scheduler: RxApp.MainThreadScheduler)
            .Bind(readOnlyObservableCollection: out _selectedItems)
            .Subscribe();
    }

    public void RemoveEditingItem(ItemState item) => _selectedItemCollection.Remove(item: item);

    public async Task<AppModelState> EditItem(ItemState item) =>
        await Dispatcher.UIThread.InvokeAsync(function: async () =>
            (await _queryDispatcher
                .Dispatch<GetProjectItemContentQuery, Resource<AppItemModel>>(
                    query: new GetProjectItemContentQuery(ItemPath: item.ItemPath),
                    cancellation: CancellationToken.None))
            .OnSuccessToTuple<AppItemModel>(onSuccessAction: success =>
            {
                if (_selectedItemCollection.Items.All(predicate: i => i.ItemPath != item.ItemPath))
                {
                    _selectedItemCollection.Add(item: item);
                }

                CurrentSelectedItem = item;


                return success?.Data as AppItemModel;
            })
            .OnError<AppItemModel>(onErrorAction: error =>
            {
                return error.Data as AppItemModel;
            })
            .value.ToAppState()
        );*/
}