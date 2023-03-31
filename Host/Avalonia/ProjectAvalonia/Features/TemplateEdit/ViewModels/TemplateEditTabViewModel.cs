using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

using Avalonia.Threading;

using Common;

using Core.Entities.Solution.Project.AppItem;
using Core.Enuns;

using Project.Domain.Contracts;
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
            if ((InEditingItem is not null) && (value.Name == InEditingItem.Name))
            {
                Dispatcher.UIThread.InvokeAsync(async () =>
                {
                    var dialog = new DeleteDialogViewModel(
                                        message: "O item seguinte será excluido ao confirmar. Deseja continuar?", title: "Deletar Item", caption: "");
                    if ((await NavigateDialogAsync(dialog, target: NavigationTarget.CompactDialogScreen)).Result == true)
                    {
                    }
                    else
                    {

                    }
                });

            }
        }
    }
    [AutoNotify] private FileItem _inEditingItem;
    [AutoNotify] private AppModelState _editingItem;

    public ObservableCollection<AppFormDataType> Types => new(
           Enum
           .GetValues<AppFormDataType>());

    private readonly IQueryDispatcher queryDispatcher;

    public TemplateEditTabViewModel()
    {
        queryDispatcher ??= Locator.Current.GetService<IQueryDispatcher>();


        LoadItemCommand = ReactiveCommand.CreateFromTask(async (FileItem item) =>
        {
            await LoadItemData(item.FilePath);
        });


        this.WhenAnyValue(vm => vm.InEditingItem)
            .WhereNotNull()
            .InvokeCommand(LoadItemCommand);

        AddFormItemCommand = ReactiveCommand.Create(() =>
        {
            Logger.LogDebug("Add Form Item");
        });

        AddLawCommand = ReactiveCommand.Create(() =>
        {
            EditingItem.LawItems.Add(new());
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
                EditingItem.LawItems.Remove(lawItem);

                Logger.LogDebug($"Remove Law {lawItem.LawId}");
            }
        });
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
            /*success?.Data?.FormData.Where(item =>
            {
                return item is AppFormDataItemCheckboxModel || item is AppFormDataItemTextModel;
            });*/

            EditingItem = success?.Data.ToAppState();
        })
        .OnError(error =>
        {
        });
    }

    private ICommand LoadItemCommand
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
