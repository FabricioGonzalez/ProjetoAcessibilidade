using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

using Avalonia;
using Avalonia.Threading;

using Common;

using Core.Entities.Solution.Project.AppItem;
using Core.Enuns;

using Project.Domain.App.Models;
using Project.Domain.Contracts;
using Project.Domain.Project.Commands.ProjectItemCommands.SaveCommands;
using Project.Domain.Project.Queries.GetProjectItemContent;

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
            "Settings", "General", "Bitcoin", "Dark", "Mode", "Run", "Computer", "System", "Start", "Background", "Close",
            "Auto", "Copy", "Paste", "Addresses", "Custom", "Change", "Address", "Fee", "Display", "Format", "BTC", "sats"
    },
    IconName = "settings_general_regular")]

public partial class TemplateEditTabViewModel : TemplateEditTabViewModelBase
{
    private FileItem _selectedItem;
    public FileItem SelectedItem
    {
        get => _selectedItem;
        set
        {
            if (value is { })
            {
                if ((InEditingItem is not null) && (value?.Name != InEditingItem.Name))
                {
                    Dispatcher.UIThread.InvokeAsync(async () =>
                    {
                        var dialog = new DeleteDialogViewModel(
                                            message: "O item seguinte será excluido ao confirmar. Deseja continuar?", title: "Deletar Item", caption: "");
                        if ((await NavigateDialogAsync(dialog, target: NavigationTarget.CompactDialogScreen)).Result == true)
                        {
                            _selectedItem = value;
                            InEditingItem = _selectedItem;
                            this.RaisePropertyChanged(nameof(SelectedItem));
                            this.RaisePropertyChanged(nameof(InEditingItem));
                        }
                        else
                        {
                            _selectedItem = InEditingItem;
                            this.RaisePropertyChanged(nameof(SelectedItem));
                        }
                    });
                }
                if (InEditingItem is null)
                {
                    _selectedItem = value;
                    InEditingItem = _selectedItem;
                    this.RaisePropertyChanged(nameof(SelectedItem));
                    this.RaisePropertyChanged(nameof(InEditingItem));
                }
            }
        }
    }

    [AutoNotify] private FileItem _inEditingItem;

    [AutoNotify] private AppModelState _editingItem;

    public ObservableCollection<AppFormDataType> Types => new()
    {AppFormDataType.Texto,
        AppFormDataType.Checkbox
    };

    private readonly IQueryDispatcher queryDispatcher;
    private readonly ICommandDispatcher commandDispatcher;

    public TemplateEditTabViewModel()
    {
        queryDispatcher ??= Locator.Current.GetService<IQueryDispatcher>();
        commandDispatcher ??= Locator.Current.GetService<ICommandDispatcher>();

        LoadItemCommand = ReactiveCommand.CreateFromTask(async (FileItem item) =>
        {
            if (item is not null)
                await LoadItemData(item.FilePath);
        });

        SaveItemCommand = ReactiveCommand.CreateFromTask(async (AppModelState item) =>
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
            EditingItem.AddLawItems(new());
            Logger.LogDebug("Add Law Item");
        });

        RemoveFormItemCommand = ReactiveCommand.Create<ReactiveObject>((item) =>
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

        RemoveLawItemCommand = ReactiveCommand.Create<LawStateItem>((lawItem) =>
        {
            if (lawItem is not null)
            {
                EditingItem.RemoveLawItem(lawItem);

                Logger.LogDebug($"Remove Law {lawItem.LawId}");
            }
        });

        ChangeItemTypeCommand = ReactiveCommand.Create<FormItemStateBase>((item) =>
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

    private async Task LoadItemData(string path)
    {
        (await queryDispatcher
            .Dispatch<GetSystemProjectItemContentQuery, Resource<AppItemModel>>(
            query: new(path),
            cancellation: CancellationToken.None))
        .OnLoadingStarted(isLoading =>
        {

        })
        .OnSuccess(success =>
        {
            EditingItem = success?.Data.ToAppState();
        })
        .OnError(error =>
        {
        });
    }
    private async Task SaveItemData(AppModelState item)
    {
        /*InEditingItem*/
        await commandDispatcher
        .Dispatch<SaveSystemProjectItemContentCommand, Resource<Empty>>(new(
            AppItem: item.ToAppModel(),
            ItemPath: InEditingItem.FilePath),
            CancellationToken.None);
    }

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
}
