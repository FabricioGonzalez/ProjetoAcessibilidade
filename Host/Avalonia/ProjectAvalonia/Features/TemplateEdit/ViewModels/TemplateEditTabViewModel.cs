using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Threading;
using Common;
using Core.Entities.Solution.Project.AppItem;
using Core.Enuns;
using Project.Domain.App.Models;
using Project.Domain.Contracts;
using Project.Domain.Project.Commands.SystemItems;
using Project.Domain.Project.Queries.SystemItems;
using ProjectAvalonia.Common.Models.FileItems;
using ProjectAvalonia.Features.Project.States;
using ProjectAvalonia.Features.Project.States.FormItemState;
using ProjectAvalonia.Features.Project.States.LawItemState;
using ProjectAvalonia.Features.Project.ViewModels.Dialogs;
using ProjectAvalonia.Logging;
using ReactiveUI;
using Splat;

namespace ProjectAvalonia.Features.TemplateEdit.ViewModels;

[NavigationMetaData(
    Title = "Template Editing",
    Caption = "Manage general settings",
    Order = 0,
    Searchable = false,
    Category = "Templates",
    Keywords = new[]
    {
        "Settings", "General", "Bitcoin", "Dark", "Mode", "Run", "Computer", "System", "Start", "Background", "Close"
        , "Auto", "Copy", "Paste", "Addresses", "Custom", "Change", "Address", "Fee", "Display", "Format", "BTC", "sats"
    },
    IconName = "settings_general_regular")]
public partial class TemplateEditTabViewModel : TemplateEditTabViewModelBase
{
    private readonly ICommandDispatcher commandDispatcher;

    private readonly IQueryDispatcher queryDispatcher;

    [AutoNotify] private AppModelState _editingItem;

    [AutoNotify] private FileItem _inEditingItem;
    private FileItem _selectedItem;

    public TemplateEditTabViewModel()
    {
        queryDispatcher ??= Locator.Current.GetService<IQueryDispatcher>();
        commandDispatcher ??= Locator.Current.GetService<ICommandDispatcher>();

        LoadItemCommand = ReactiveCommand.CreateFromTask(execute: async (
            FileItem item
        ) =>
        {
            if (item is not null)
            {
                await LoadItemData(path: item.FilePath);
            }
        });

        SaveItemCommand = ReactiveCommand.CreateFromTask(execute: async (
            AppModelState item
        ) =>
        {
            if (item is not null)
            {
                await SaveItemData(item: item);
            }
        });

        AddFormItemCommand = ReactiveCommand.Create(execute: () =>
        {
            Logger.LogDebug(message: "Add Form Item");
        });

        AddLawCommand = ReactiveCommand.Create(execute: () =>
        {
            EditingItem.AddLawItems(lawItem: new LawStateItem());
            Logger.LogDebug(message: "Add Law Item");
        });

        RemoveFormItemCommand = ReactiveCommand.Create<ReactiveObject>(execute: item =>
        {
            if (item is CheckboxContainerItemState checkbox)
            {
                Logger.LogDebug(message: $"Remove Item {checkbox.Topic}");
            }

            if (item is TextItemState textItem)
            {
                Logger.LogDebug(message: $"Remove Item {textItem.Topic}");
            }
        });

        RemoveLawItemCommand = ReactiveCommand.Create<LawStateItem>(execute: lawItem =>
        {
            if (lawItem is not null)
            {
                EditingItem.RemoveLawItem(lawItem: lawItem);

                Logger.LogDebug(message: $"Remove Law {lawItem.LawId}");
            }
        });

        ChangeItemTypeCommand = ReactiveCommand.Create<FormItemStateBase>(execute: item =>
        {
            if (item is not null && EditingItem.FormData.IndexOf(value: item) != -1)
            {
                var res = EditingItem.FormData.IndexOf(value: item);
                Logger.LogDebug(message: $"Remove Law {item.Type} at {res}");
            }
        });

        this
            .WhenAnyValue(property1: vm => vm.InEditingItem)
            .InvokeCommand(command: LoadItemCommand);
    }

    public FileItem SelectedItem
    {
        get => _selectedItem;
        set
        {
            if (value is not null)
            {
                if (InEditingItem is not null && value?.Name != InEditingItem.Name)
                {
                    Dispatcher.UIThread.InvokeAsync(function: async () =>
                    {
                        var dialog = new DeleteDialogViewModel(
                            message: "O item seguinte será excluido ao confirmar. Deseja continuar?"
                            , title: "Deletar Item", caption: "");
                        if ((await NavigateDialogAsync(dialog: dialog, target: NavigationTarget.CompactDialogScreen))
                            .Result)
                        {
                            _selectedItem = value;
                            InEditingItem = _selectedItem;
                            this.RaisePropertyChanged();
                            this.RaisePropertyChanged(propertyName: nameof(InEditingItem));
                        }
                        else
                        {
                            _selectedItem = InEditingItem;
                            this.RaisePropertyChanged();
                        }
                    });
                }

                if (InEditingItem is null)
                {
                    _selectedItem = value;
                    InEditingItem = _selectedItem;
                    this.RaisePropertyChanged();
                    this.RaisePropertyChanged(propertyName: nameof(InEditingItem));
                }
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
                query: new GetSystemProjectItemContentQuery(ItemPath: path),
                cancellation: CancellationToken.None))
        .OnSuccess(onSuccessAction: success =>
        {
            EditingItem = success?.Data.ToAppState();
        })
        .OnError(onErrorAction: error =>
        {
        });

    private async Task SaveItemData(
        AppModelState item
    ) =>
        /*InEditingItem*/
        await commandDispatcher
            .Dispatch<SaveSystemProjectItemContentCommand, Resource<Empty>>(
                command: new SaveSystemProjectItemContentCommand(
                    AppItem: item.ToAppModel(),
                    ItemPath: InEditingItem.FilePath),
                cancellation: CancellationToken.None);
}