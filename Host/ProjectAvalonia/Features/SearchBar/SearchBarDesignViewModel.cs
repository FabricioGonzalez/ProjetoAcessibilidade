using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using DynamicData;
using ProjectAvalonia.Features.SearchBar.Patterns;
using ProjectAvalonia.Features.SearchBar.SearchItems;
using ReactiveUI;

namespace ProjectAvalonia.Features.SearchBar;

public class SearchBarDesignViewModel : ReactiveObject
{
    private readonly IEnumerable<ISearchItem> _items;

    public SearchBarDesignViewModel()
    {
        static Task PreventExecution()
        {
            return Task.Run(action: () =>
            {
            });
        }

        var actionable = new IActionableItem[]
        {
            new ActionableItem(name: "Test 1: Short", description: "Description short", onExecution: PreventExecution
                , category: "Settings")
            {
                Icon = "settings_bitcoin_regular"
            }
            , new ActionableItem(name: "Test 2: Loooooooooooong", description: "Description long"
                , onExecution: PreventExecution, category: "Settings")
            {
                Icon = "settings_bitcoin_regular"
            }
            , new ActionableItem(
                name: "Test 3: Short again",
                description: "Description very very loooooooooooong and difficult to read",
                onExecution: PreventExecution,
                category: "Settings")
            {
                Icon = "settings_bitcoin_regular"
            }
            , new ActionableItem(name: "Test 3", description: "Another", onExecution: PreventExecution
                , category: "Settings")
            {
                Icon = "settings_bitcoin_regular"
            }
            , new ActionableItem(
                name: "Test 4: Help topics",
                description: "Description very very loooooooooooong and difficult to read",
                onExecution: PreventExecution,
                category: "Help")
            {
                Icon = "settings_bitcoin_regular"
            }
            , new ActionableItem(name: "Test 3", description: "Another", onExecution: PreventExecution
                , category: "Help")
            {
                Icon = "settings_bitcoin_regular"
            }
        };

        _items = actionable.ToList();
    }

    public ReadOnlyObservableCollection<SearchItemGroup> Groups => new(
        list: new ObservableCollection<SearchItemGroup>(
            collection: _items
                .GroupBy(keySelector: r => r.Category)
                .Select(
                    selector: grouping =>
                    {
                        var sourceCache = new SourceCache<ISearchItem, ComposedKey>(keySelector: r => r.Key);
                        sourceCache.PopulateFrom(observable: grouping.ToObservable());
                        return new SearchItemGroup(title: grouping.Key, changes: sourceCache.Connect());
                    })));

    public string SearchText => "";
}