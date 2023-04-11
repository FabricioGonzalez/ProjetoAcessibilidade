using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Entities.App;
using DynamicData;
using Project.Domain.App.Queries.UF;
using Project.Domain.Contracts;
using ReactiveUI;

namespace ProjectAvalonia.Stores;

public partial class AddressesStore
{
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly SourceList<UFModel> _ufList;

    [AutoNotify] public ReadOnlyObservableCollection<UFModel> UfList;

    public AddressesStore(IQueryDispatcher queryDispatcher)
    {
        _queryDispatcher = queryDispatcher;
        _ufList = new SourceList<UFModel>();

        _ufList.Connect()
            .ObserveOn(scheduler: RxApp.MainThreadScheduler)
            .Bind(readOnlyObservableCollection: out UfList);
    }

    public async Task LoadAllUf(CancellationToken token)
    {
        foreach (var item in (await _queryDispatcher
                     .Dispatch<GetAllUfQuery, IList<UFModel>>(query: new GetAllUfQuery()
                         , cancellation: token)
                 ).OrderBy(keySelector: x => x.Name))
        {
            if (_ufList.Items.All(predicate: i => i.Code != item.Code))
            {
                _ufList.Add(item: item);
            }
        }
    }
}