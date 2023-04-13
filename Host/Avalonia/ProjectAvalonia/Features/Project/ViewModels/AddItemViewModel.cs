using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Avalonia.Threading;
using Project.Domain.Contracts;
using ProjectAvalonia.Features.Project.States.ProjectItems;
using ProjectAvalonia.Stores;
using ProjectAvalonia.ViewModels.Dialogs.Base;
using ReactiveUI;
using Splat;

namespace ProjectAvalonia.Features.Project.ViewModels;

[NavigationMetaData(Title = "Adicionar item")]
public partial class AddItemViewModel : DialogViewModelBase<ItemState>
{
    private readonly TemplateItemsStore? _itemsStore;
    private readonly IQueryDispatcher? _queryDispatcher;

    [AutoNotify] private ItemState? _item;

    [AutoNotify] private string _itemName = "";
    /*{
        get;
    }*/

    public AddItemViewModel()
    {
        SetupCancel(enableCancel: true, enableCancelOnEscape: true, enableCancelOnPressed: true);

        _queryDispatcher ??= Locator.Current.GetService<IQueryDispatcher>();
        _itemsStore ??= Locator.Current.GetService<TemplateItemsStore>();

        this.WhenAnyValue(property1: vm => vm.Item)
            .WhereNotNull()
            .Subscribe(onNext: item =>
            {
                if (string.IsNullOrWhiteSpace(value: ItemName))
                {
                    ItemName = item.TemplateName;
                }
            });

        NextCommand = ReactiveCommand.Create(
            execute: OnNext,
            canExecute: this.WhenAnyValue(property1: x => x.Item)
                .Select(selector: prop => prop is not null)
                .ObserveOn(scheduler: RxApp.MainThreadScheduler));


        Dispatcher.UIThread.Post(action: async () =>
        {
            await _itemsStore?.LoadSystemItems(token: GetCancellationToken());
        });
    }

    public ReadOnlyObservableCollection<ItemState> Items => _itemsStore.ItemsCollection;


    private void OnNext()
    {
        Item.Name = ItemName;
        Close(kind: DialogResultKind.Normal, result: Item);
    }
}