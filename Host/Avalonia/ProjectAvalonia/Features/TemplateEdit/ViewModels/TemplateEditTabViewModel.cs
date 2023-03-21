using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

using Common;

using Core.Entities.Solution.Project.AppItem;
using Core.Enuns;

using Project.Domain.Contracts;
using Project.Domain.Project.Queries.GetProjectItemContent;

using ProjectAvalonia.Common.Models.FileItems;
using ProjectAvalonia.Features.Project.States;
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
    [AutoNotify] private FileItem _selectedItem;
    [AutoNotify] private AppModelState _editingItem;

    public ObservableCollection<AppFormDataType> Types => new(
           Enum
           .GetValues<AppFormDataType>());

    private readonly IQueryDispatcher queryDispatcher;

    public TemplateEditTabViewModel()
    {
        queryDispatcher ??= Locator.Current.GetService<IQueryDispatcher>();

        this.WhenAnyValue(vm => vm.SelectedItem)
            .WhereNotNull()
            .Subscribe(async (prop) =>
            {
                await LoadItemData(prop.FilePath);
                /*Logger.LogDebug(prop.Name);*/
            });
        AddFormItemCommand = ReactiveCommand.Create(() =>
        {
            Logger.LogDebug("Add Form Item");
        });

        AddLawCommand = ReactiveCommand.Create(() =>
        {
            Logger.LogDebug("Add Law Item");
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

    public ICommand AddFormItemCommand
    {
        get;
    }
    public ICommand AddLawCommand
    {
        get;
    }
}
