using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using DynamicData;
using ProjectAvalonia.Common.ViewModels;
using ProjectAvalonia.Features.NavBar;
using ProjectAvalonia.Features.SearchBar.Patterns;
using ProjectAvalonia.Features.SearchBar.SearchItems;
using ProjectAvalonia.Features.SearchBar.Sources;
using ProjectAvalonia.Features.SearchBars.ViewModels.SearchBar.Sources;
using ProjectAvalonia.ViewModels.Navigation;

namespace ProjectAvalonia.Features.SearchBar.SearchBar.Sources;

public class ActionsSearchSource : ISearchSource
{
    public ActionsSearchSource(
        IObservable<string> query
    )
    {
        var filter = query.Select(SearchSource.DefaultFilter);

        Changes = GetItemsFromMetadata()
            .ToObservable()
            .ToObservableChangeSet(x => x.Key)
            .Filter(filter);
    }

    public IObservable<IChangeSet<ISearchItem, ComposedKey>> Changes
    {
        get;
    }

    private static IEnumerable<ISearchItem> GetItemsFromMetadata() =>
        NavigationManager.MetaData
            .Where(m => m.Searchable)
            .Select(m =>
            {
                var onActivate = CreateOnActivateFunction(m);
                var searchItem = new ActionableItem(m.Title,
                    m.Caption,
                    onActivate,
                    m.Category ?? "No category",
                    m.Keywords)
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
            var vm = await NavigationManager.MaterialiseViewModelAsync(navigationMetaData);
            if (vm is null)
            {
                return;
            }

            if (vm is NavBarItemViewModel item && item.OpenCommand.CanExecute.Wait())
            {
                item.OpenCommand.Execute(default);
            }
            else if (vm is TriggerCommandViewModel triggerCommandViewModel
                     && triggerCommandViewModel.TargetCommand.CanExecute(default))
            {
                triggerCommandViewModel.TargetCommand.Execute(default);
            }
            else
            {
                RoutableViewModel
                    .Navigate(vm.DefaultTarget)
                    .To(vm);
            }
        };
}