using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;

using ReactiveUI;

namespace AppUsecases.Entities.FileTemplate;
public class ExplorerItem
{
    public string Name
    {
        get; set;
    }
    public string Path
    {
        get; set;
    }
    public ExplorerItemType Type
    {
        get; set;
    }
    public IList<ExplorerItem> Children
    {
        get;set;
    }
    public ICommand RenameItemCommand
    {
        get; private set;
    } = ReactiveCommand.Create(() =>
    {
        Debug.WriteLine("RenameItem");
    });

    public ICommand ExcludeItemCommand
    {
        get; private set;
    } = ReactiveCommand.Create(() =>
    {
        Debug.WriteLine("ExcludeItem");
    });

    public ICommand AddItemCommand
    {
        get; private set;
    } = ReactiveCommand.Create(() =>
    {
        Debug.WriteLine("AddItem");
    });

    public ICommand AddFolderCommand
    {
        get; private set;
    } = ReactiveCommand.Create(() =>
    {
        Debug.WriteLine("AddFolder");
    });

}
