using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Windows.Input;

using AppUsecases.Contracts.Repositories;
using AppUsecases.Contracts.Usecases;
using AppUsecases.Editing.Entities;
using AppUsecases.Project.Entities.FileTemplate;
using AppUsecases.Project.Entities.Project;

using Common;

using DynamicData;
using DynamicData.Binding;

using Project.Core.ComposableViewModel;
using Project.Core.ViewModels.Extensions;

using ReactiveUI;

using Splat;

using UIStatesStore.App.Models;
using UIStatesStore.Contracts;

using UIStatesStore.Project.Models;

namespace Project.Core.ViewModels.Project;
public class ExplorerComponentViewModel : ViewModelBase
{
    private SourceList<FolderItem> _items;

    private ObservableCollectionExtended<ProjectItemViewModel> explorerItems = new();
    public ObservableCollectionExtended<ProjectItemViewModel> ExplorerItems
    {
        get => explorerItems;
        set => this.RaiseAndSetIfChanged(ref explorerItems, value, nameof(ExplorerItems));
    }
    public ObservableCollection<ProjectItemViewModel> SelectedItems
    {
        get; set;
    }

    private UIStatesStore.Solution.Models.ProjectSolutionModel solutionModel = new();
    public UIStatesStore.Solution.Models.ProjectSolutionModel SolutionModel
    {
        get => solutionModel;
        set => this.RaiseAndSetIfChanged(ref solutionModel, value, nameof(SolutionModel));

    }

    string strFolder = "";

    public string Folder
    {
        get => strFolder;
        set => this.RaiseAndSetIfChanged(ref strFolder, value, nameof(Folder));
    }

    bool isDocumentSolutionEnabled = false;

    public bool IsDocumentSolutionEnabled
    {
        get => isDocumentSolutionEnabled;
        set => this.RaiseAndSetIfChanged(ref isDocumentSolutionEnabled, value, nameof(IsDocumentSolutionEnabled));
    }

    private readonly IAppObservable<ProjectModel> projectState;
    private readonly IAppObservable<AppErrorMessage> AppErrorState;
    readonly IQueryUsecase<string, List<ExplorerItem>> getProjectItems;
    private readonly IAppObservable<ProjectEditingModel> ProjectEditingObservable;
    IQueryUsecase<string, AppItemModel> getItemContent;
    private readonly IAppObservable<UIStatesStore.Solution.Models.ProjectSolutionModel> solutionObserver;

    public ExplorerComponentViewModel()
    {
        projectState ??= Locator.Current.GetService<IAppObservable<ProjectModel>>();
        AppErrorState ??= Locator.Current.GetService<IAppObservable<AppErrorMessage>>();
        ProjectEditingObservable ??= Locator.Current.GetService<IAppObservable<ProjectEditingModel>>();
        solutionObserver ??= Locator.Current.GetService<IAppObservable<UIStatesStore.Solution.Models.ProjectSolutionModel>>();

        getProjectItems ??= Locator.Current.GetService<IQueryUsecase<string, List<ExplorerItem>>>();
        getItemContent ??= Locator.Current.GetService<IQueryUsecase<string, AppItemModel>>();

        ShowDialog = new Interaction<AddItemViewModel, FileTemplate?>();

        AddItemCommand = ReactiveCommand.CreateFromTask<ProjectItemViewModel, Unit>(async (item) =>
        {
            var store = new AddItemViewModel();

            FileTemplate? result = await ShowDialog.Handle(store);

            if (result is not null)
            {
                ((FolderProjectItemViewModel)item).Children.Add(new FileProjectItemViewModel()
                {
                    Title = result.Name,
                    Path = result.FilePath,
                    InEditMode = true
                });
            }

            return Unit.Default;
        });

        AddFolderCommand = ReactiveCommand.CreateFromTask<ProjectItemViewModel, Unit>(async (item) =>
        {
            if (item is not null)
            {
                ((FolderProjectItemViewModel)item).Children.Add(new FolderProjectItemViewModel()
                {
                    Title = "",
                    Path = item.Path,
                    InEditMode = true
                });
            }
            return Unit.Default;

        });

        SelectSolutionItemCommand = ReactiveCommand.CreateFromTask<string, Unit>(async (item) =>
        {
            var result = await getItemContent.executeAsync(item);

            result
            .OnLoading(out var itemModel, out var isLoading)
                    .OnError(out itemModel, out var error)
                    .OnSuccess(out itemModel);

            ProjectEditingObservable.Send(new(itemModel.ItemName, itemModel));

            return new Unit();
        });

        this.WhenActivated((disposables) =>
        {
            this.WhenAnyValue(x => x.Folder)
                    .Subscribe(path =>
                    {
                        if (path is not null && path.Length > 0)
                        {
                            var result = getProjectItems.executeAsync(path).Result;

                            result
                            .OnError(out var items, out var error)
                            .OnLoading(out items, out var isLoading)
                            .OnSuccess(out items);


                            ExplorerItems = new(GetSubfolders(items));
                        }
                    })
                    .DisposeWith(disposables);

            solutionObserver.Subscribe(x =>
            {
                SolutionModel = x;
            }).DisposeWith(disposables);

            projectState.Subscribe(x =>
                        {
                            Folder = x.ProjectPath;
                        })
                        .DisposeWith(disposables);
        });
    }

    public ReactiveCommand<ProjectItemViewModel, Unit> AddItemCommand
    {
        get; private set;
    }

    public ReactiveCommand<ProjectItemViewModel, Unit> AddFolderCommand
    {
        get; private set;
    }
    public ReactiveCommand<string, Unit> SelectSolutionItemCommand
    {
        get; private set;
    }
    public Interaction<AddItemViewModel, FileTemplate?> ShowDialog
    {
        get; private set;
    }

    private List<ProjectItemViewModel> GetSubfolders(List<ExplorerItem> items)
    {
        List<ProjectItemViewModel> subfolders = new();

        if (items is not null && items.Count > 0)
        {

            foreach (var dir in items)
            {
                ProjectItemViewModel thisnode = new FileProjectItemViewModel()
                {
                    Title = dir.Name,
                    Path = dir.Path,
                    InEditMode = false,
                };

                if (dir is FolderItem item)
                {
                    thisnode = new FolderProjectItemViewModel()
                    {
                        Title = item.Name,
                        Path = item.Path,
                        InEditMode = false,
                    };

                    (thisnode as FolderProjectItemViewModel).Children = new(GetSubfolders(item.Children));
                }

                subfolders.Add(thisnode);
            }
        }

        return subfolders;
    }
}
