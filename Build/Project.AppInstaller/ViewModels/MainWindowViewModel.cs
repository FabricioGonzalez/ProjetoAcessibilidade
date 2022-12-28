using System;
using System.Collections.ObjectModel;
using System.IO;

using DynamicData.Binding;

namespace Project.AppInstaller.ViewModels;
public class MainWindowViewModel : ViewModelBase
{
    public string Greeting => "Welcome to Avalonia!";

    public ObservableCollectionExtended<Node> Files { get; } = new();

    public MainWindowViewModel()
    {
        var directory = Environment.CurrentDirectory;

        Files = new();

        Node rootNode = new Node(directory);
        rootNode.Subfolders = GetSubfolders(directory);

        Files.Add(rootNode);
    }
    public ObservableCollection<Node> GetSubfolders(string strPath)
    {
        ObservableCollection<Node> subfolders = new ObservableCollection<Node>();
        string[] subdirs = Directory.GetFileSystemEntries(strPath);

        foreach (string dir in subdirs)
        {
            Node thisnode = new Node(dir);

            if (File.GetAttributes(dir).HasFlag(FileAttributes.Directory))
            {
                if (Directory.GetFileSystemEntries(dir).Length > 0)
                {
                    thisnode.Subfolders = new ObservableCollection<Node>();

                    thisnode.Subfolders = GetSubfolders(dir);
                }
            }

            subfolders.Add(thisnode);
        }

        return subfolders;
    }
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
