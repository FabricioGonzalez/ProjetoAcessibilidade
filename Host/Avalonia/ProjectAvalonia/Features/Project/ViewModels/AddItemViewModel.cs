﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.Presentation.States.ProjectItems;
using ProjectAvalonia.ViewModels;
using ProjectAvalonia.ViewModels.Dialogs.Base;
using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels;

[NavigationMetaData(Title = "Adicionar item")]
public partial class AddItemViewModel
    : DialogViewModelBase<ItemState>
        , IAddItemViewModel
{
    /*private readonly TemplateItemsStore? _itemsStore;*/
    /*private readonly IMediator? _mediator;*/

    [AutoNotify] private string _itemName = "";

    [AutoNotify] private ItemState? _selectedItem;

    public AddItemViewModel(
        ObservableCollection<IItemViewModel> fileItems
    )
    {
        SetupCancel(enableCancel: true, enableCancelOnEscape: true, enableCancelOnPressed: true);

        /*_mediator ??= Locator.Current.GetService<IMediator>();*/
        /*_itemsStore ??= Locator.Current.GetService<TemplateItemsStore>();*/

        var canCreateItem = this.WhenAnyValue(vm => vm.ItemName)
            .Select(name => !string.IsNullOrEmpty(name)
                            && !fileItems.Any(x => x.Name == name)
                            && SelectedItem != null);

        this.WhenAnyValue(vm => vm.SelectedItem)
            .WhereNotNull()
            .Subscribe(item =>
            {
                if (string.IsNullOrWhiteSpace(ItemName))
                {
                    ItemName = item.TemplateName;
                }
            });

        LoadAllItems = ReactiveCommand.CreateFromTask(execute: async () =>
            {
                /*(await _mediator?
                        .Send(
                            new GetAllTemplatesQuery(),
                            CancellationToken.None))
                    ?.OnSuccess(success =>
                    {
                        Items = success
                            ?.Data
                            ?.Select(item => new ItemState
                            {
                                Id = item.Id ?? Guid.NewGuid().ToString(),
                                ItemPath = item.ItemPath,
                                Name = ""
                                ,
                                TemplateName = item.Name
                            }) ?? Enumerable.Empty<ItemState>();

                        this.RaisePropertyChanged(nameof(Items));
                    })
                    ?.OnError(error =>
                    {
                    });*/
            }
            , outputScheduler: RxApp.MainThreadScheduler);

        NextCommand = ReactiveCommand.Create(
            execute: OnNext,
            canExecute: canCreateItem);
    }

    public override string? LocalizedTitle
    {
        get;
        protected set;
    }

    public override MenuViewModel? ToolBar => null;

    public IEnumerable<ItemState> Items
    {
        get;
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