using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Windows.Input;

using AppUsecases.Contracts.Repositories;
using AppUsecases.Contracts.Usecases;
using AppUsecases.Project.Entities.FileTemplate;

using Common;

using DynamicData;
using DynamicData.Binding;

using Project.Core.ComposableViewModel;

using ReactiveUI;

using Splat;

using UIStatesStore.App.Models;
using UIStatesStore.Contracts;

using UIStatesStore.Project.Models;

namespace Project.Core.ViewModels;
public class ExplorerComponentViewModel : ViewModelBase
{
    private SourceList<FolderItem> _items;

    private ObservableCollectionExtended<ProjectItemViewModel> explorerItems = new();
    public ObservableCollectionExtended<ProjectItemViewModel> ExplorerItems
    {
        get => explorerItems; set => this.RaiseAndSetIfChanged(ref explorerItems, value, nameof(ExplorerItems));
    }
    public ObservableCollection<ProjectItemViewModel> SelectedItems
    {
        get; set;
    }

    string strFolder = "";

    public string Folder
    {
        get => strFolder; set => this.RaiseAndSetIfChanged(ref strFolder, value, nameof(Folder));
    }

    private readonly IAppObservable<ProjectModel> projectState;
    private readonly IAppObservable<AppErrorMessage> AppErrorState;
    readonly IQueryUsecase<string, List<ExplorerItem>> getProjectItems;
    public ExplorerComponentViewModel()
    {
        projectState ??= Locator.Current.GetService<IAppObservable<ProjectModel>>();
        AppErrorState ??= Locator.Current.GetService<IAppObservable<AppErrorMessage>>();
        getProjectItems ??= Locator.Current.GetService<IQueryUsecase<string, List<ExplorerItem>>>();

        ShowDialog = new Interaction<AddItemViewModel, ExplorerComponentViewModel?>();

        AddItemCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var store = new AddItemViewModel();

            var result = await ShowDialog.Handle(store);
        });

        this.WhenActivated((CompositeDisposable disposables) =>
        {
            this.WhenAnyValue(x => x.Folder)
                    .Subscribe(path =>
                    {
                        if (path is not null && path.Length > 0)
                        {
                            var error = "";
                            var isLoading = false;
                            var items = new List<ExplorerItem>();
                            var result = getProjectItems.executeAsync(path).Result;

                            result.OnError(ref items, ref error)
                            .OnLoading(ref items, ref isLoading)
                            .OnSuccess(ref items);


                            ExplorerItems = new(GetSubfolders(items));
                        }
                    })
                    .DisposeWith(disposables);

            projectState.Subscribe(x =>
            {
                Folder = x.ProjectPath;
            })
            .DisposeWith(disposables);
        });
    }

    public ReactiveCommand<Unit, Unit> AddItemCommand
    {
        get; private set;
    }
    public ReactiveCommand<ProjectItemViewModel, Unit> RenameItemCommand
    {
        get; private set;
    }
    public Interaction<AddItemViewModel, ExplorerComponentViewModel?> ShowDialog
    {
        get; private set;
    }

    public List<ProjectItemViewModel> GetSubfolders(List<ExplorerItem> items)
    {
        List<ProjectItemViewModel> subfolders = new();
        foreach (var dir in items)
        {
            ProjectItemViewModel thisnode = new FileProjectItemViewModel()
            {
                Title = dir.Name,
                InEditMode = false,
            };

            if (dir is FolderItem item)
            {
                thisnode = new FolderProjectItemViewModel()
                {
                    Title = item.Name,
                    InEditMode = false,
                };

                (thisnode as FolderProjectItemViewModel).Children = new();

                (thisnode as FolderProjectItemViewModel).Children = GetSubfolders(item.Children);
            }

            subfolders.Add(thisnode);
        }

        return subfolders;
    }
}
