namespace AppUsecases.Project.Entities.FileTemplate;
public class FolderItem : ExplorerItem
{
    public List<ExplorerItem> Children
    {
        get; set;
    }
}
