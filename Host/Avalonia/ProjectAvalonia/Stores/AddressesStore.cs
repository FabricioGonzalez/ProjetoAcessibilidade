using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Entities.App;
using DynamicData;
using ProjetoAcessibilidade.Domain.App.Queries.UF;
using ProjetoAcessibilidade.Domain.Contracts;
using ReactiveUI;

namespace ProjectAvalonia.Stores;

public partial class AddressesStore
{
    private readonly IMediator _queryDispatcher;
    private readonly SourceList<UFModel> _ufList;

    [AutoNotify] public ReadOnlyObservableCollection<UFModel> UfList;

    public AddressesStore(
        IMediator queryDispatcher
    )
    {
        _queryDispatcher = queryDispatcher;
        _ufList = new SourceList<UFModel>();

        _ufList.Connect()
            .ObserveOn(scheduler: RxApp.MainThreadScheduler)
            .Bind(readOnlyObservableCollection: out UfList);
    }

    public async Task LoadAllUf(
        CancellationToken token
    )
    {
        foreach (var item in (await _queryDispatcher
                     .Send(request: new GetAllUfQuery()
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