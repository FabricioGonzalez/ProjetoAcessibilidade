using System;
using System.Collections.ObjectModel;
using System.IO;
using DynamicData.Binding;

namespace Project.AppInstaller.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
        var directory = Environment.CurrentDirectory;

        Files = new ObservableCollectionExtended<Node>();

        var rootNode = new Node(_strFullPath: directory);
        rootNode.Subfolders = GetSubfolders(strPath: directory);

        Files.Add(item: rootNode);
    }

    public string Greeting => "Welcome to Avalonia!";

    public ObservableCollectionExtended<Node> Files
    {
        get;
    } = new();

    public ObservableCollection<Node> GetSubfolders(
        string strPath
    )
    {
        var subfolders = new ObservableCollection<Node>();
        var subdirs = Directory.GetFileSystemEntries(path: strPath);

        foreach (var dir in subdirs)
        {
            var thisnode = new Node(_strFullPath: dir);

            if (File.GetAttributes(path: dir).HasFlag(flag: FileAttributes.Directory))
            {
                if (Directory.GetFileSystemEntries(path: dir).Length > 0)
                {
                    thisnode.Subfolders = new ObservableCollection<Node>();

                    thisnode.Subfolders = GetSubfolders(strPath: dir);
                }
            }

            subfolders.Add(item: thisnode);
        }

        return subfolders;
    }
}

public class Node
{
    public Node(
        string _strFullPath
    )
    {
        strFullPath = _strFullPath;
        strNodeText = Path.GetFileName(path: _strFullPath);
    }

    public ObservableCollection<Node> Subfolders
    {
        get;
        set;
    }

    public string strNodeText
    {
        get;
    }

    public string strFullPath
    {
        get;
    }
}