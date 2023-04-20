using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using Common;
using Core.Entities.Solution.ItemsGroup;
using Project.Domain.App.Queries.Templates;
using Project.Domain.Contracts;
using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.Presentation.States.ProjectItems;
using ProjectAvalonia.ViewModels.Dialogs.Base;
using ReactiveUI;
using Splat;

namespace ProjectAvalonia.Features.Project.ViewModels;

[NavigationMetaData(Title = "Adicionar item")]
public partial class AddItemViewModel : DialogViewModelBase<ItemState>, IAddItemViewModel
{
    /*private readonly TemplateItemsStore? _itemsStore;*/
    private readonly IQueryDispatcher? _queryDispatcher;

    [AutoNotify] private string _itemName = "";

    [AutoNotify] private ItemState? _selectedItem;

    public AddItemViewModel()
    {
        SetupCancel(enableCancel: true, enableCancelOnEscape: true, enableCancelOnPressed: true);

        _queryDispatcher ??= Locator.Current.GetService<IQueryDispatcher>();
        /*_itemsStore ??= Locator.Current.GetService<TemplateItemsStore>();*/

        this.WhenAnyValue(property1: vm => vm.SelectedItem)
            .WhereNotNull()
            .Subscribe(onNext: item =>
            {
                if (string.IsNullOrWhiteSpace(value: ItemName))
                {
                    ItemName = item.TemplateName;
                }
            });

        LoadAllItems = ReactiveCommand.CreateFromTask(execute: async () =>
            {
                (await _queryDispatcher?
                        .Dispatch<GetAllTemplatesQuery, Resource<List<ItemModel>>>(
                            query: new GetAllTemplatesQuery(),
                            cancellation: CancellationToken.None))
                    ?.OnSuccess(onSuccessAction: success =>
                    {
                        Items = success
                            ?.Data
                            ?.Select(selector: item => new ItemState
                            {
                                Id = item.Id ?? Guid.NewGuid().ToString(),
                                ItemPath = item.ItemPath,
                                Name = "",
                                TemplateName = item.Name
                            }) ?? Enumerable.Empty<ItemState>();

                        this.RaisePropertyChanged(propertyName: nameof(Items));
                    })
                    ?.OnError(onErrorAction: error =>
                    {
                    });
            }
            , outputScheduler: RxApp.MainThreadScheduler);

        NextCommand = ReactiveCommand.Create(
            execute: OnNext,
            canExecute: this.WhenAnyValue(property1: x => x.SelectedItem)
                .Select(selector: prop => prop is not null)
                .ObserveOn(scheduler: RxApp.MainThreadScheduler));
    }

    public IEnumerable<ItemState> Items
    {
        get;
        private set;
    }

    public ReactiveCommand<Unit, Unit> LoadAllItems
    {
        get;
    }


    private void OnNext()
    {
        SelectedItem.Name = ItemName;
        Close(kind: DialogResultKind.Normal, result: SelectedItem);
    }
}