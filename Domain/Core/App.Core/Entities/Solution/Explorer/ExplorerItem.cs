namespace ProjetoAcessibilidade.Core.Entities.Solution.Explorer;

public class ExplorerItem : BaseAuditableEntity
{
    public ExplorerItem(
        Guid id
    ) : base(id)
    {
    }

    public string Name
    {
        get;
        set;
    }

    public string Path
    {
        get;
        set;
    }

    public string ReferencedItem
    {
        get;
        set;
    }
}