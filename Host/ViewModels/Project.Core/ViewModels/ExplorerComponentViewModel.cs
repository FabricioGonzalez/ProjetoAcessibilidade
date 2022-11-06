using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Runtime;
using System.Windows.Input;

using AppUsecases.Contracts.Repositories;
using AppUsecases.Contracts.Usecases;
using AppUsecases.Project.Entities.FileTemplate;

using Common;

using DynamicData;
using DynamicData.Binding;

using ReactiveUI;

using Splat;

using UIStatesStore.App.Models;
using UIStatesStore.Contracts;

using UIStatesStore.Project.Models;

namespace Project.Core.ViewModels;
public class ExplorerComponentViewModel : ViewModelBase
{
    private SourceList<FolderItem> _items;

    private ObservableCollectionExtended<ExplorerItem> explorerItems = new();
    public ObservableCollectionExtended<ExplorerItem> ExplorerItems
    {
        get => explorerItems; set => this.RaiseAndSetIfChanged(ref explorerItems, value, nameof(ExplorerItems));
    }
    public ObservableCollection<ExplorerItem> SelectedItems
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
   
    IQueryUsecase<string, List<ExplorerItem>> getProjectItems;
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
                    .Subscribe( path =>
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

                            ExplorerItems = new (items);
                        }
                    });

            projectState.Subscribe(x =>
            {
                Folder = x.ProjectPath;
            });          
        });
    }

    public ReactiveCommand<Unit,Unit> AddItemCommand
    {
        get; private set;
    }

    public Interaction<AddItemViewModel, ExplorerComponentViewModel?> ShowDialog
    {
        get;private set;
    }

    public ObservableCollection<ExplorerItem> GetSubfolders(string strPath)
    {
        ObservableCollection<ExplorerItem> subfolders = new ObservableCollection<ExplorerItem>();
        string[] subdirs = Directory.GetDirectories(strPath, "*", SearchOption.TopDirectoryOnly);

        foreach (string dir in subdirs)
        {
            ExplorerItem thisnode = new FileItem()
            {
                Name = dir.Split(Path.DirectorySeparatorChar)[dir.Split(Path.DirectorySeparatorChar).Length - 1],
                Path = dir,
            };

            if (Directory.GetDirectories(dir, "*", SearchOption.TopDirectoryOnly).Length > 0)
            {
                thisnode = new FolderItem()
                {
                    Name = dir.Split(Path.DirectorySeparatorChar)[dir.Split(Path.DirectorySeparatorChar).Length - 1],
                    Path = dir,
                };

                (thisnode as FolderItem).Children = new ObservableCollection<ExplorerItem>();

                (thisnode as FolderItem).Children = GetSubfolders(dir);
            }

            subfolders.Add(thisnode);
        }

        return subfolders;
    }
}
