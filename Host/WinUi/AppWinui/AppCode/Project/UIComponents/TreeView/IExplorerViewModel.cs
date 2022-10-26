using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using AppUsecases.Entities.FileTemplate;

namespace AppWinui.AppCode.Project.UIComponents.TreeView;
public interface IExplorerViewModel
{
    public ExplorerItem Item
    {
        get;
        set ;
    }

    public ICommand RenameItemCommand
    {
        get;  set;
    }

    public ICommand ExcludeItemCommand
    {
        get;  set;
    }

    public ICommand AddItemCommand
    {
        get;  set;
    }

    public ICommand AddFolderCommand
    {
        get;  set;
    }
}
