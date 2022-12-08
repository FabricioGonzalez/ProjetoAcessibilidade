using AppViewModels.Project.ComposableViewModels;
using AppViewModels.Project.Mappers;

namespace AppViewModels.Project.Operations;
public class ProjectExplorerOperations
{
    public ProjectExplorerOperations()
    {

    }
    public FileProjectItemViewModel RenameFile(FileProjectItemViewModel item,List<ProjectItemViewModel> items)
    {
        var file = items.SearchFile(item);


        return item;
    }

    public FolderProjectItemViewModel RenameFolder(FolderProjectItemViewModel item, List<ProjectItemViewModel> items)
    {
        var folder = items.SearchFolder(item);

        return item;
    }

    public FileProjectItemViewModel DeleteFile(FileProjectItemViewModel item, List<ProjectItemViewModel> items)
    {
        var file = items.SearchFile(item);

        return item;
    }

    public FolderProjectItemViewModel DeleteFolder(FolderProjectItemViewModel item, List<ProjectItemViewModel> items)
    {
        var folder = items.SearchFolder(item);

        return item;
    }

}
