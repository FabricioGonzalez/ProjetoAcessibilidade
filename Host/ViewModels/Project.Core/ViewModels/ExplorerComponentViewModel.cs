using System.Collections.ObjectModel;

using AppUsecases.Project.Entities.FileTemplate;

using ReactiveUI;

namespace Project.Core.ViewModels;
public class ExplorerComponentViewModel : ViewModelBase
{
    /*    private IEnumerable<object> explorerItems;

        public IEnumerable<object> ExplorerItems
        {
            get => explorerItems;
            set => this.RaiseAndSetIfChanged(ref explorerItems, value);
        }
    */
    public ObservableCollection<FolderItem> Items
    {
        get; set;
    }
    public ObservableCollection<FolderItem> SelectedItems
    {
        get; set;
    }
    public string strFolder
    {
        get; set;
    }

    public ExplorerComponentViewModel()
    {
        strFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}{Path.DirectorySeparatorChar}"; // EDIT THIS FOR AN EXISTING FOLDER

        Items = new ObservableCollection<FolderItem>();

        FolderItem rootNode = new FolderItem()
        {
            Name = strFolder.Split(Path.DirectorySeparatorChar)[strFolder.Split(Path.DirectorySeparatorChar).Length - 1],
            Path = strFolder,
        };
        rootNode.Children = GetSubfolders(strFolder);

        Items.Add(rootNode);
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

    public class Node
    {
        public ObservableCollection<Node> Subfolders
        {
            get; set;
        }

        public string strNodeText
        {
            get;
        }
        public string strFullPath
        {
            get;
        }

        public Node(string _strFullPath)
        {
            strFullPath = _strFullPath;
            strNodeText = Path.GetFileName(_strFullPath);
        }
    }
}
