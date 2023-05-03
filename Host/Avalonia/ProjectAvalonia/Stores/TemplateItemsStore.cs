namespace ProjectAvalonia.Stores;

public class TemplateItemsStore
{
    /*private readonly IMediator? _queryDispatcher;

    private readonly SourceList<ItemState> _systemItems;

    [AutoNotify] private ReadOnlyObservableCollection<ItemState> _itemsCollection;

    public TemplateItemsStore(
        IMediator queryDispatcher
    )
    {
        _queryDispatcher ??= queryDispatcher;

        _systemItems = new SourceList<ItemState>();

        _systemItems
            .Connect()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Sort(SortExpressionComparer<ItemState>.Ascending(state => state.Name))
            .Bind(out _itemsCollection)
            .Subscribe();
    }

    public async Task LoadSystemItems(
        CancellationToken token
    ) =>
        (await _queryDispatcher?
            .Dispatch<GetAllTemplatesQuery, Resource<List<ItemModel>>>(
                new GetAllTemplatesQuery(),
                token))
        ?.OnSuccess(success =>
        {
            foreach (var item in success
                         ?.Data
                         ?.Select(item => new ItemState
                         {
                             Id = item.Id ?? Guid.NewGuid().ToString(),
                             ItemPath = item.ItemPath,
                             Name = "",
                             TemplateName = item.Name
                         }) ?? Enumerable.Empty<ItemState>())
            {
                if (_systemItems.Items.All(i => /*i.Id != item.Id ||#1# i.TemplateName != item.TemplateName))
                {
                    _systemItems.Add(item);
                }
            }
        })
        ?.OnError(error =>
        {
        });

    public void RemoveItem(
        ItemState item
    ) =>
        _systemItems.Remove(item);

    public void AddItem(
        ItemState item
    ) =>
        _systemItems.Add(item);*/
}