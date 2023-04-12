using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Core.Entities.Solution.ItemsGroup;
using DynamicData;
using DynamicData.Binding;
using Project.Domain.App.Queries.Templates;
using Project.Domain.Contracts;
using ProjectAvalonia.Features.Project.States.ProjectItems;
using ReactiveUI;

namespace ProjectAvalonia.Stores;

public partial class TemplateItemsStore
{
    private readonly IQueryDispatcher? _queryDispatcher;

    private readonly SourceList<ItemState> _systemItems;

    [AutoNotify] private ReadOnlyObservableCollection<ItemState> _itemsCollection;

    public TemplateItemsStore(
        IQueryDispatcher queryDispatcher
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
                if (_systemItems.Items.All(i => /*i.Id != item.Id ||*/ i.TemplateName != item.TemplateName))
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
        _systemItems.Add(item);
}