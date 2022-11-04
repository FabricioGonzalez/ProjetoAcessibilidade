using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive.Disposables;

using AppUsecases.Project.Entities.FileTemplate;

using ReactiveUI;

using Splat;
using UIStatesStore.Contracts;

using UIStatesStore.Project.Models;
using UIStatesStore.Project.Observable;

namespace Project.Core.ViewModels;
public class ExplorerComponentViewModel : ViewModelBase
{
    public ObservableCollection<ExplorerItem> ExplorerItems
    {
        get; set;
    }
    public ObservableCollection<ExplorerItem> SelectedItems
    {
        get; set;
    }
    string strFolder = "";

    public string Folder
    {
        get => strFolder; set => this.RaiseAndSetIfChanged(ref strFolder,value);
    }

    IAppObservable<ProjectModel> projectState;

    public ExplorerComponentViewModel()
    {
        ExplorerItems = new ObservableCollection<ExplorerItem>();
        projectState = Locator.Current.GetService<IAppObservable<ProjectModel>>();

        this.WhenActivated((CompositeDisposable disposables) =>
        {
            this.WhenAnyValue(x => x.Folder)
            .Subscribe(path =>
            {
                if (path is not null && path.Length > 0)
                {
                   
                    FolderItem rootNode = new FolderItem()
                    {
                        Name = path.Split(Path.DirectorySeparatorChar)[path.Split(Path.DirectorySeparatorChar).Length - 1],
                        Path = path,
                    };

                    var folder = string.Join(Path.DirectorySeparatorChar, path.Split(Path.DirectorySeparatorChar)[..(path.Split(Path.DirectorySeparatorChar).Length - 1)]);

                    rootNode.Children = GetSubfolders(folder);

                    ExplorerItems.Add(rootNode);
                }
            });

            projectState.Subscribe(x =>
            {
                Folder = x.ProjectPath;
                Debug.WriteLine($"chosen path: {x.ProjectPath}");
            })
            .DisposeWith(disposables);
        });
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
