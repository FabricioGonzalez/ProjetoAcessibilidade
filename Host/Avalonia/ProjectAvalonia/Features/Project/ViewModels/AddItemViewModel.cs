using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using Common;
using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.Presentation.States.ProjectItems;
using ProjectAvalonia.ViewModels.Dialogs.Base;
using ProjetoAcessibilidade.Domain.App.Queries.Templates;
using ProjetoAcessibilidade.Domain.Contracts;
using ReactiveUI;
using Splat;

namespace ProjectAvalonia.Features.Project.ViewModels;

[NavigationMetaData(Title = "Adicionar item")]
public partial class AddItemViewModel
    : DialogViewModelBase<ItemState>
        , IAddItemViewModel
{
    /*private readonly TemplateItemsStore? _itemsStore;*/
    private readonly IMediator? _queryDispatcher;

    [AutoNotify] private string _itemName = "";

    [AutoNotify] private ItemState? _selectedItem;

    public AddItemViewModel()
    {
        SetupCancel(true, true, true);

        _queryDispatcher ??= Locator.Current.GetService<IMediator>();
        /*_itemsStore ??= Locator.Current.GetService<TemplateItemsStore>();*/

        this.WhenAnyValue(vm => vm.SelectedItem)
            .WhereNotNull()
            .Subscribe(item =>
            {
                if (string.IsNullOrWhiteSpace(ItemName))
                {
                    ItemName = item.TemplateName;
                }
            });

        LoadAllItems = ReactiveCommand.CreateFromTask(async () =>
            {
                (await _queryDispatcher?
                        .Dispatch(
                            new GetAllTemplatesQuery(),
                            CancellationToken.None))
                    ?.OnSuccess(success =>
                    {
                        Items = success
                            ?.Data
                            ?.Select(item => new ItemState
                            {
                                Id = item.Id ?? Guid.NewGuid().ToString(), ItemPath = item.ItemPath, Name = ""
                                , TemplateName = item.Name
                            }) ?? Enumerable.Empty<ItemState>();

                        this.RaisePropertyChanged(nameof(Items));
                    })
                    ?.OnError(error =>
                    {
                    });
            }
            , outputScheduler: RxApp.MainThreadScheduler);

        NextCommand = ReactiveCommand.Create(
            OnNext,
            this.WhenAnyValue(x => x.SelectedItem)
                .Select(prop => prop is not null)
                .ObserveOn(RxApp.MainThreadScheduler));
    }

    public override string? LocalizedTitle
    {
        get;
        protected set;
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
        Close(DialogResultKind.Normal, SelectedItem);
    }
}