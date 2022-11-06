namespace AppUsecases.Project.Entities.FileTemplate;
public class FolderItem : ExplorerItem
{
    public IList<ExplorerItem> Children
    {
        get; set;
    }
}
