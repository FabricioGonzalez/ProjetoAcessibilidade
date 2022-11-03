using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using AppUsecases.Project.Entities.FileTemplate;

using AppWinui.AppCode.Project.UIComponents.TreeView;

using ReactiveUI;

namespace AppWinui.AppCode.Project.ViewModels;
public class FileItemViewModel : ReactiveObject, IExplorerViewModel
{
    private ExplorerItem item;

    public ExplorerItem Item
    {
        get => item;
        set => this.RaiseAndSetIfChanged(ref item, value);
    }
    public FileItemViewModel()
    {
        RenameItemCommand = ReactiveCommand.Create(() =>
        {
            Debug.WriteLine("Rename");
        });

        ExcludeItemCommand = ReactiveCommand.Create(() =>
        {
            Debug.WriteLine("Exclude");
        });
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
        get;
        set;
    }
    public ICommand AddFolderCommand
    {
        get;
        set;
    }
}
