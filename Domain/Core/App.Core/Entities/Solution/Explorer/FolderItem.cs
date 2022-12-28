namespace App.Core.Entities.Solution.Explorer;
public class FolderItem : ExplorerItem
{
    public List<ExplorerItem> Children
    {
        get; set;
    } = new();
}
