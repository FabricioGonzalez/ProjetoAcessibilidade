namespace ProjetoAcessibilidade.Core.Entities.Solution.Explorer;

public class FolderItem : ExplorerItem
{
    public FolderItem(
        Guid id
    ) : base(id)
    {
    }

    public List<ExplorerItem> Children
    {
        get;
        set;
    } = new();
}