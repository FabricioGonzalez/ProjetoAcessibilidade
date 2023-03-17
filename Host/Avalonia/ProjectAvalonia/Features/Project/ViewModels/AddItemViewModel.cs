using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

using Avalonia.Threading;

using Common;

using Core.Entities.Solution.Explorer;

using DynamicData.Binding;

using Project.Domain.App.Queries.GetAllTemplates;
using Project.Domain.Contracts;

using ProjectAvalonia.Features.Project.States.ProjectItems;
using ProjectAvalonia.ViewModels.Dialogs.Base;

using ReactiveUI;

using Splat;

namespace ProjectAvalonia.Features.Project.ViewModels;

[NavigationMetaData(Title = "Adicionar item")]
public partial class AddItemViewModel : DialogViewModelBase<ItemState>
{

    [AutoNotify]
    private ObservableCollectionExtended<ItemState> _items;

    [AutoNotify]
    private ItemState _item;

    [AutoNotify]
    private string _itemName = "";

    private readonly IQueryDispatcher queryDispatcher;

    public AddItemViewModel()
    {
        SetupCancel(enableCancel: true, enableCancelOnEscape: true, enableCancelOnPressed: true);

        queryDispatcher ??= Locator.Current.GetService<IQueryDispatcher>();

        this.WhenAnyValue(vm => vm.Item)
            .WhereNotNull()
            .Subscribe(item =>
            {
                if (string.IsNullOrWhiteSpace(ItemName))
                    ItemName = Item.TemplateName;
            });

        NextCommand = ReactiveCommand.Create(
            OnNext,
            this.WhenAnyValue(x => x.Item)
            .Select(prop => prop is not null)
            .ObserveOn(RxApp.MainThreadScheduler));


        Dispatcher.UIThread.Post(async () =>
        {
            await GetItems();
        });
    }

    public async Task GetItems()
    {
        var result = await queryDispatcher
            .Dispatch<GetAllTemplatesQuery, Resource<List<ExplorerItem>>>(new(),
            CancellationToken.None);

        result
            .OnSuccess((success) =>
            {
                Items = new(
                    success
                    ?.Data
                    ?.Select(item => new ItemState()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ItemPath = item.Path,
                        Name = "",
                        TemplateName = item.Name
                    }) ?? Enumerable.Empty<ItemState>());
            })
            .OnError((error) =>
             {

             })
            .OnLoadingStarted((loading) => { })
            .OnLoadingEnded((loading) => { });



    }

    private void OnNext()
    {
        Item.Name = ItemName;
        Close(DialogResultKind.Normal, Item);
    }

    public ReactiveCommand<Unit, Unit> CloseDialogCommand
    {
        get; set;
    }
}
