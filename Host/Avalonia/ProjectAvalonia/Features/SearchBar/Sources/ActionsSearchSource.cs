using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using DynamicData;
using ProjectAvalonia.Features.NavBar;
using ProjectAvalonia.Features.SearchBar.Patterns;
using ProjectAvalonia.Features.SearchBar.SearchItems;
using ProjectAvalonia.Features.SearchBar.Sources;
using ProjectAvalonia.Features.SearchBars.ViewModels.SearchBar.Sources;
using ProjectAvalonia.ViewModels;
using ProjectAvalonia.ViewModels.Navigation;

namespace ProjectAvalonia.Features.SearchBar.SearchBar.Sources;

public class ActionsSearchSource : ISearchSource
{
    public ActionsSearchSource(
        IObservable<string> query
    )
    {
        var filter = query.Select(selector: SearchSource.DefaultFilter);

        Changes = GetItemsFromMetadata()
            .ToObservable()
            .ToObservableChangeSet(keySelector: x => x.Key)
            .Filter(predicateChanged: filter);
    }

    public IObservable<IChangeSet<ISearchItem, ComposedKey>> Changes
    {
        get;
    }

    private static IEnumerable<ISearchItem> GetItemsFromMetadata() =>
        NavigationManager.MetaData
            .Where(predicate: m => m.Searchable)
            .Select(selector: m =>
            {
                var onActivate = CreateOnActivateFunction(navigationMetaData: m);
                var searchItem = new ActionableItem(name: m.Title,
                    description: m.Caption,
                    onExecution: onActivate,
                    category: m.Category ?? "No category",
                    keywords: m.Keywords)
                {
                    Icon = m.IconName, IsDefault = true
                };
                return searchItem;
            });

    private static Func<Task> CreateOnActivateFunction(
        NavigationMetaData navigationMetaData
    ) =>
        async () =>
        {
            var vm = await NavigationManager.MaterialiseViewModelAsync(metaData: navigationMetaData);
            if (vm is null)
            {
                return;
            }

            if (vm is NavBarItemViewModel item && item.OpenCommand.CanExecute(parameter: default))
            {
                item.OpenCommand.Execute(parameter: default);
            }
            else if (vm is TriggerCommandViewModel triggerCommandViewModel
                     && triggerCommandViewModel.TargetCommand.CanExecute(parameter: default))
            {
                triggerCommandViewModel.TargetCommand.Execute(parameter: default);
            }
            else
            {
                RoutableViewModel
                    .Navigate(currentTarget: vm.DefaultTarget)
                    .To(viewmodel: vm);
            }
        };
}