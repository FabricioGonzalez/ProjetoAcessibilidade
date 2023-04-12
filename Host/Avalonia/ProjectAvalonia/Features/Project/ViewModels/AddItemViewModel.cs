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

    [AutoNotify]
    private ItemState? _item;

    [AutoNotify]
    private string _itemName = "";
    /*{
        get;
    }*/

    public AddItemViewModel()
    {
        SetupCancel(true, true, true);

        _queryDispatcher ??= Locator.Current.GetService<IQueryDispatcher>();
        _itemsStore ??= Locator.Current.GetService<TemplateItemsStore>();

        this.WhenAnyValue(vm => vm.Item)
            .WhereNotNull()
            .Subscribe(item =>
            {
                if (string.IsNullOrWhiteSpace(ItemName))
                {
                    ItemName = Item.TemplateName;
                }
            });

        NextCommand = ReactiveCommand.Create(
            OnNext,
            this.WhenAnyValue(x => x.Item)
                .Select(prop => prop is not null)
                .ObserveOn(RxApp.MainThreadScheduler));


        Dispatcher.UIThread.Post(async () =>
        {
            await _itemsStore?.LoadSystemItems(GetCancellationToken());
        });
    }

    public ReadOnlyObservableCollection<ItemState> Items => _itemsStore.ItemsCollection;


    private void OnNext()
    {
        Item.Name = ItemName;
        Close(DialogResultKind.Normal, Item);
    }
}