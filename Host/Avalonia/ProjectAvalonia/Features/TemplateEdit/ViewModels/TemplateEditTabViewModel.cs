using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

using Common.Optional;

using DynamicData;
using DynamicData.Binding;

using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.Presentation.States;
using ProjectAvalonia.Presentation.States.FormItemState;
using ProjectAvalonia.Presentation.States.LawItemState;
using ProjectAvalonia.ViewModels;

using ProjetoAcessibilidade.Core.Enuns;

using ReactiveUI;

namespace ProjectAvalonia.Features.TemplateEdit.ViewModels;

[NavigationMetaData(
    Title = "Template Editing",
    Caption = "Manage general settings",
    Order = 0,
    Searchable = false,
    Category = "Templates",
    Keywords = new[]
    {
        "Settings", "General", "Bitcoin", "Dark", "Mode", "Run", "Computer", "System", "Start", "Background", "Close",
        "Auto", "Copy", "Paste", "Addresses", "Custom", "Change", "Address", "Fee", "Display", "Format", "BTC", "sats"
    },
    IconName = "settings_general_regular")]
public partial class TemplateEditTabViewModel
    : TemplateEditTabViewModelBase,
        ITemplateEditTabViewModel
{
    /*private readonly ICommandDispatcher commandDispatcher;

    private readonly IMediator queryDispatcher;

    [AutoNotify] private AppModelState _editingItem;

    [AutoNotify] private ItemState? _inEditingItem;

    private ItemState _selectedItem;

    public TemplateEditTabViewModel()
    {
        queryDispatcher ??= Locator.Current.GetService<IMediator>();
        commandDispatcher ??= Locator.Current.GetService<ICommandDispatcher>();

        LoadItemCommand = ReactiveCommand.CreateFromTask(async (
            ItemState? item
        ) =>
        {
            if (item is not null)
            {
                await LoadItemData(item.ItemPath);
            }
        });

        SaveItemCommand = ReactiveCommand.CreateFromTask(async (
            AppModelState? item
        ) =>
        {
            if (item is not null)
            {
                await SaveItemData(item);
            }
        });

        AddFormItemCommand = ReactiveCommand.Create(() =>
        {
            Logger.LogDebug("Add Form Item");
        });

        AddLawCommand = ReactiveCommand.Create(() =>
        {
            EditingItem.AddLawItems(new LawStateItem());
            Logger.LogDebug("Add Law Item");
        });

        RemoveFormItemCommand = ReactiveCommand.Create<FormItemStateBase?>(item =>
        {
            if (item is CheckboxContainerItemState checkbox)
            {
                Logger.LogDebug($"Remove Item {checkbox.Topic}");
            }

            if (item is TextItemState textItem)
            {
                Logger.LogDebug($"Remove Item {textItem.Topic}");
            }
        });

        RemoveLawItemCommand = ReactiveCommand.Create<LawStateItem?>(lawItem =>
        {
            if (lawItem is not null)
            {
                EditingItem.RemoveLawItem(lawItem);

                Logger.LogDebug($"Remove Law {lawItem.LawId}");
            }
        });

        ChangeItemTypeCommand = ReactiveCommand.Create<FormItemStateBase?>(item =>
        {
            if (item is not null && EditingItem.FormData.IndexOf(item) != -1)
            {
                var res = EditingItem.FormData.IndexOf(item);
                Logger.LogDebug($"Remove Law {item.Type} at {res}");
            }
        });

        this
            .WhenAnyValue(vm => vm.InEditingItem)
            .InvokeCommand(LoadItemCommand);
    }

    public ItemState? SelectedItem
    {
        get => _selectedItem;
        set
        {
            if (InEditingItem is not null && value?.TemplateName != InEditingItem.TemplateName)
            {
                Dispatcher.UIThread.InvokeAsync(async () =>
                {
                    var dialog = new DeleteDialogViewModel(
                        "O item seguinte será excluido ao confirmar. Deseja continuar?"
                        , "Deletar Item", "");
                    if ((await NavigateDialogAsync(dialog, NavigationTarget.CompactDialogScreen))
                        .Result)
                    {
                        InEditingItem = value;
                        this.RaiseAndSetIfChanged(ref _selectedItem, value);
                    }
                });
            }

            if (InEditingItem is null)
            {
                InEditingItem = value;
                this.RaiseAndSetIfChanged(ref _selectedItem, value);
            }
        }
    }

    public ObservableCollection<AppFormDataType> Types => new()
    {
        AppFormDataType.Texto, AppFormDataType.Checkbox
    };

    private ICommand SaveItemCommand
    {
        get;
    }

    private ICommand LoadItemCommand
    {
        get;
    }

    public ICommand ChangeItemTypeCommand
    {
        get;
    }

    public ICommand AddFormItemCommand
    {
        get;
    }

    public ICommand RemoveFormItemCommand
    {
        get;
    }

    public ICommand AddLawCommand
    {
        get;
    }

    public ICommand RemoveLawItemCommand
    {
        get;
    }

    private async Task LoadItemData(
        string path
    ) =>
        (await queryDispatcher
            .Dispatch<GetSystemProjectItemContentQuery, Resource<AppItemModel>>(
                new GetSystemProjectItemContentQuery(path),
                CancellationToken.None))
        .OnSuccess(success =>
        {
            EditingItem = success?.Data.ToAppState();
        })
        .OnError(error =>
        {
        });

    private async Task SaveItemData(
        AppModelState item
    ) =>
        /*InEditingItem#1#
        await commandDispatcher
            .Dispatch<SaveSystemProjectItemContentCommand, Resource<Empty>>(
                new SaveSystemProjectItemContentCommand(
                    item.ToAppModel(),
                    InEditingItem.ItemPath),
                CancellationToken.None);*/

    public TemplateEditTabViewModel()
    {
        var observable = this.WhenAnyValue(vm => vm.EditingItem)
            .Select(x => x.FormData
            .ToObservableChangeSet()
            .AutoRefresh()
            .DisposeMany())
            .Switch()
            .WhenPropertyChanged(prop => prop.Type, notifyOnInitialValue: false)
            .Subscribe(onNext: prop =>
              {
                  ChangeBody(prop.Sender);
              }, onError: error =>
              {
                  Debug.WriteLine(error);
              },
            onCompleted: () =>
            {
                Debug.WriteLine("Completado");
            });
    }
    public override MenuViewModel? ToolBar => null;
    private void ChangeBody(FormItemContainer container)
    {

        var result = container.Body.ToOption()
            .Map(e => e = container.Type switch
            {
                AppFormDataType.Texto => container.ChangeItem(type: AppFormDataType.Texto)
                .Reduce(orElse: () => new TextItemState(topic: container.Body.Topic, textData: "", id: container.Body.Id)),

                AppFormDataType.Checkbox => container.ChangeItem(type: AppFormDataType.Checkbox)
                .Reduce(orElse: () => new CheckboxContainerItemState(topic: container.Body.Topic, id: container.Body.Id))
            })
            .Reduce(() => container.Type switch
            {
                AppFormDataType.Texto => container.ChangeItem(type: AppFormDataType.Texto)
                .Reduce(orElse: () => new TextItemState(topic: "", textData: "", id: Guid.NewGuid().ToString())),

                AppFormDataType.Checkbox => container.ChangeItem(type: AppFormDataType.Checkbox)
                .Reduce(orElse: () => new CheckboxContainerItemState(topic: "", id: Guid.NewGuid().ToString()))
            });
        container.Body = result;

        container.AddToHistory(result);
    }


    public ReactiveCommand<Unit, Unit> AddItemCommand => ReactiveCommand.Create(() =>
    {
        EditingItem.AddFormItem(new()
        {
            Id = Guid.NewGuid().ToString(),
            Topic = "",
            Type = AppFormDataType.Texto,
            Body = new TextItemState(topic: "", textData: "", id: Guid.NewGuid().ToString())
        });
    });
    public ReactiveCommand<Unit, Unit> AddLawCommand => ReactiveCommand.Create(() =>
    {
        EditingItem.AddLawItems(new());
    });

    public ReactiveCommand<FormItemContainer, Unit> RemoveItemCommand => ReactiveCommand.Create<FormItemContainer>((item) =>
    {
        EditingItem.RemoveItem(item);
    });

    public ReactiveCommand<LawStateItem, Unit> RemoveLawCommand => ReactiveCommand.Create<LawStateItem>((item) =>
    {
        EditingItem.RemoveLawItem(item);
    });

    public override string? LocalizedTitle
    {
        get;
        protected set;
    } = null;

    private AppModelState _editingItem;
    public AppModelState EditingItem
    {
        get => _editingItem;
        set => this.RaiseAndSetIfChanged(ref _editingItem, value);
    }
}
