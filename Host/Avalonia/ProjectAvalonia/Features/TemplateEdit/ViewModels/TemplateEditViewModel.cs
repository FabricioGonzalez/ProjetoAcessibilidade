using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

using Common;

using Core.Entities.Solution.Explorer;

using DynamicData;
using DynamicData.Binding;

using Project.Application.App.Queries.GetAllTemplates;
using Project.Application.Contracts;

using ProjectAvalonia.Features.NavBar;

using ReactiveUI;

using Splat;

using FileItem = ProjectAvalonia.Common.Models.FileItems.FileItem;

namespace ProjectAvalonia.Features.TemplateEdit.ViewModels;

[NavigationMetaData(
    Title = "Template Edit",
    Caption = "Edit project items templates",
    Order = 0,
    Category = "Templates",
    Searchable = true,
    Keywords = new[]
    {
            "Templates", "Editing"
    },
        NavBarPosition = NavBarPosition.Top,
    NavigationTarget = NavigationTarget.HomeScreen,
    IconName = "edit_regular")]
public partial class TemplateEditViewModel : NavBarItemViewModel
{
    [AutoNotify] private int _selectedTab;
    [AutoNotify] private ReadOnlyObservableCollection<FileItem> _items;
    public ObservableCollectionExtended<FileItem> Source
    {
        get;
    } = new();

    private readonly IQueryDispatcher queryDispatcher;

    public async Task LoadItems()
    {
        var result = await queryDispatcher
            .Dispatch<GetAllTemplatesQuery, Resource<List<ExplorerItem>>>(
            query: new(),
            cancellation: CancellationToken.None);

        result.OnError((res) =>
        {

        })
        .OnLoadingStarted((res) =>
        {
        })
        .OnSuccess((res) =>
        {
            if (res.Data is not null)
            {
                Source.Load(
                    res.Data.Select(item => new FileItem()
                    {
                        Name = item.Name,
                        FilePath = item.Path
                    }));
            }
        });
    }

    public TemplateEditViewModel()
    {
        queryDispatcher = Locator.Current.GetService<IQueryDispatcher>();

        Source.ToObservableChangeSet()
             .ObserveOn(RxApp.MainThreadScheduler)
             .Bind(out _items)
             .Subscribe();

        SelectionMode = NavBarItemSelectionMode.Button;

        _selectedTab = 0;

        TemplateEditTab = new TemplateEditTabViewModel();


    }
    public TemplateEditTabViewModel TemplateEditTab
    {
        get;
    }

    public ICommand AddNewItemCommand
    {
        get;
    }
    public ICommand CommitItemCommand
    {
        get;
    }
}
